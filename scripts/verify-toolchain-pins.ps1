$ErrorActionPreference = 'Stop'

$dockerfile = Get-Content (Join-Path $PSScriptRoot '..' 'Dockerfile.vercel') -Raw
$workflowDirectory = Join-Path $PSScriptRoot '..' '.github' 'workflows'
$workflowFiles = @(Get-ChildItem -LiteralPath $workflowDirectory -File |
    Where-Object Extension -in @('.yml', '.yaml'))
if ($workflowFiles.Count -ne 1 -or $workflowFiles[0].Name -ne 'ci.yml') {
    throw 'CI must be the only workflow; unreviewed deployment workflows are not allowed.'
}
$workflow = Get-Content -LiteralPath $workflowFiles[0].FullName -Raw
$toolManifest = Get-Content (Join-Path $PSScriptRoot '..' '.config' 'dotnet-tools.json') -Raw |
    ConvertFrom-Json

$requiredPatterns = @(
    'mcr\.microsoft\.com/dotnet/sdk:\d+\.\d+\.\d+@sha256:[a-f0-9]{64}\s+AS\s+build',
    'mcr\.microsoft\.com/dotnet/aspnet:\d+\.\d+\.\d+@sha256:[a-f0-9]{64}\s+AS\s+runtime'
)

foreach ($pattern in $requiredPatterns) {
    if ($dockerfile -notmatch $pattern) {
        throw "Dockerfile.vercel must pin each .NET image to an exact servicing tag and manifest digest."
    }
}

$sdkPattern = @'
dotnet-version:\s*['"]\d+\.\d+\.\d+['"]
'@
if ($workflow -notmatch $sdkPattern) {
    throw 'CI must install an exact .NET SDK version rather than a floating channel.'
}

$ciSdkVersion = [regex]::Match(
    [regex]::Match($workflow, $sdkPattern).Value,
    '\d+\.\d+\.\d+').Value
$dockerSdkVersion = [regex]::Match(
    $dockerfile,
    'mcr\.microsoft\.com/dotnet/sdk:(?<version>\d+\.\d+\.\d+)@sha256:').Groups['version'].Value
if ($ciSdkVersion -ne $dockerSdkVersion) {
    throw "CI ($ciSdkVersion) and Docker ($dockerSdkVersion) must use the same .NET SDK version."
}

$projectFiles = Get-ChildItem (Join-Path $PSScriptRoot '..') -Filter '*.csproj' -Recurse -File |
    Where-Object { $_.FullName -notmatch '[\\/](?:bin|obj)[\\/]' }
$targetFrameworks = @($projectFiles | ForEach-Object {
    $project = [xml](Get-Content -LiteralPath $_.FullName -Raw)
    @($project.Project.PropertyGroup.TargetFramework) +
        @($project.Project.PropertyGroup.TargetFrameworks) |
        Where-Object { -not [string]::IsNullOrWhiteSpace([string]$_) } |
        ForEach-Object { [string]$_ }
} | Sort-Object -Unique)
if ($targetFrameworks.Count -ne 1 -or $targetFrameworks[0] -ne 'net8.0') {
    throw "All projects must target net8.0; found: $($targetFrameworks -join ', ')."
}

if ($workflow -match '(?m)^\s*runs-on:\s*\S+-latest\s*$') {
    throw 'CI runner operating systems must use an explicit release rather than a latest alias.'
}

$serviceImages = [regex]::Matches(
    $workflow,
    '(?m)^\s+image:\s*(?<reference>[^\s#]+)')
if ($serviceImages.Count -eq 0) {
    throw 'CI must define its PostgreSQL integration-test service image.'
}
foreach ($serviceImage in $serviceImages) {
    if ($serviceImage.Groups['reference'].Value -notmatch '^[^@\s]+:\S+@sha256:[a-f0-9]{64}$') {
        throw "CI service images must use a reviewed tag and immutable manifest digest: $($serviceImage.Groups['reference'].Value)"
    }
}

$actionReferences = [regex]::Matches($workflow, '(?m)^\s*-\s+uses:\s*(\S+)')
if ($actionReferences.Count -eq 0) {
    throw 'CI must contain at least one externally pinned action.'
}
foreach ($reference in $actionReferences) {
    if ($reference.Groups[1].Value -notmatch '^[^@\s]+@[a-f0-9]{40}$') {
        throw "External GitHub Actions must be pinned to a full commit SHA: $($reference.Groups[1].Value)"
    }
}

$efVersion = [string]$toolManifest.tools.'dotnet-ef'.version
if ($efVersion -notmatch '^\d+\.\d+\.\d+$') {
    throw 'dotnet-ef must use an exact three-part version in the tool manifest.'
}

Write-Host 'Toolchain pin verification passed.'
