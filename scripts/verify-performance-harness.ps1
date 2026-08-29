$ErrorActionPreference = 'Stop'
$harnessPath = Join-Path $PSScriptRoot 'measure-api.ps1'
$scenarioPath = Join-Path $PSScriptRoot 'performance-scenarios.example.json'
$tokens = $null
$parseErrors = $null
[System.Management.Automation.Language.Parser]::ParseFile(
    $harnessPath,
    [ref]$tokens,
    [ref]$parseErrors) | Out-Null
if ($parseErrors.Count -gt 0) {
    throw "Performance harness contains PowerShell parse errors: $($parseErrors.Message -join '; ')"
}

$harness = Get-Content -Raw -LiteralPath $harnessPath
foreach ($measurementInvariant in @(
    "Headers['X-Vercel-Cache']",
    "Headers['Age']",
    'ApplicationTimingDiscarded',
    'DiscardedAppTimingSamples',
    'CacheHits')) {
    if (-not $harness.Contains($measurementInvariant)) {
        throw "Performance harness is missing cache/timing invariant: $measurementInvariant"
    }
}

$scenarios = @(Get-Content -Raw -LiteralPath $scenarioPath | ConvertFrom-Json)
if ($scenarios.Count -lt 4 -or
    -not ($scenarios | Where-Object Authenticated) -or
    -not ($scenarios | Where-Object { $_.Method -ne 'GET' -and $_.Iterations -eq 1 }) -or
    -not ($scenarios | Where-Object { $_.Path -like '*{{ownerUsername}}*' })) {
    throw 'Performance examples must cover public reads, authenticated reads, and an authenticated mutation.'
}

$seederPath = Join-Path $PSScriptRoot '..\tools\Portfolio.PerformanceSeeder\Program.cs'
$seeder = Get-Content -Raw -LiteralPath $seederPath
foreach ($safetyInvariant in @(
    'PERFORMANCE_DATABASE_URL_UNPOOLED',
    'SEED_ISOLATED_PREVIEW',
    '--expected-database',
    '--expected-host',
    'GetPendingMigrationsAsync',
    'MaximumExistingUsers')) {
    if (-not $seeder.Contains($safetyInvariant)) {
        throw "Performance seeder is missing safety invariant: $safetyInvariant"
    }
}

Remove-Item Env:PERFORMANCE_BEARER_TOKEN -ErrorAction SilentlyContinue
$manifestPath = Join-Path ([IO.Path]::GetTempPath()) "portfolio-performance-manifest-$([Guid]::NewGuid()).json"
$manifest = @{
    ownerUsername = 'perf-owner'
    counts = @{
        confirmedUsers = 10000; ownerProjects = 20; ownerExperiences = 10
        ownerEducations = 10; ownerCertificates = 20; ownerSkills = 30; ownerContactMessages = 100
    }
} | ConvertTo-Json -Depth 5
[IO.File]::WriteAllText($manifestPath, $manifest, [Text.UTF8Encoding]::new($false))
try {
    & $harnessPath `
        -BaseUrl 'https://preview.invalid' `
        -Iterations 1 `
        -ScenariosFile $scenarioPath `
        -DatasetManifest $manifestPath
    throw 'Performance harness accepted authenticated scenarios without a bearer-token environment variable.'
}
catch {
    if ($_.Exception.Message -notlike '*PERFORMANCE_BEARER_TOKEN*') {
        throw
    }
}
finally {
    Remove-Item -LiteralPath $manifestPath -ErrorAction SilentlyContinue
}

Write-Host 'Performance harness verification passed.'
