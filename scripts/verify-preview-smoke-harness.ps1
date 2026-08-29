$ErrorActionPreference = 'Stop'
$harnessPath = Join-Path $PSScriptRoot 'smoke-preview.ps1'
$tokens = $null
$parseErrors = $null
[System.Management.Automation.Language.Parser]::ParseFile(
    $harnessPath,
    [ref]$tokens,
    [ref]$parseErrors) | Out-Null
if ($parseErrors.Count -gt 0) {
    throw "Preview smoke harness contains PowerShell parse errors: $($parseErrors.Message -join '; ')"
}

$expected = @(
    'liveness', 'readiness', 'health-alias', 'legacy-public-read',
    'explicit-v1-public-read', 'v1-shape-compatibility', 'unsupported-v2',
    'validation-problem-details', 'owner-authentication-challenge',
    'maintenance-credential-challenge'
)
$actual = @(& $harnessPath -ListChecks)
if (($actual -join '|') -ne ($expected -join '|')) {
    throw 'Preview smoke harness no longer exposes the complete required acceptance inventory.'
}

$source = Get-Content -Raw -LiteralPath $harnessPath
foreach ($required in @(
    'VERCEL_AUTOMATION_BYPASS_SECRET', 'x-vercel-protection-bypass',
    'UseVercelCli', 'VercelCliVersion', 'vercel@$VercelCliVersion',
    'System.Text.Encoding',
    'application/problem\+json', 'X-Correlation-ID', 'WWW-Authenticate',
    '/api/v1/Client/UserList', '/api/v2/Client/UserList')) {
    if ($source -notmatch [regex]::Escape($required)) {
        throw "Preview smoke harness is missing required invariant: $required"
    }
}

Write-Host 'Preview smoke harness verification passed.'
