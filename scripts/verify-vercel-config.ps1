$ErrorActionPreference = 'Stop'

$repositoryRoot = Join-Path $PSScriptRoot '..'
$productionPath = Join-Path $repositoryRoot 'vercel.json'
$previewPath = Join-Path $repositoryRoot 'vercel.preview.json'
$ignorePath = Join-Path $repositoryRoot '.vercelignore'
$production = Get-Content -Raw -LiteralPath $productionPath | ConvertFrom-Json
$preview = Get-Content -Raw -LiteralPath $previewPath | ConvertFrom-Json

$expectedCrons = @{
    '/api/maintenance/email-outbox' = '17 7 * * *'
    '/api/maintenance/cleanup' = '17 3 * * *'
}

if ($production.crons.Count -ne $expectedCrons.Count) {
    throw 'Production Vercel configuration must contain the reviewed outbox and cleanup schedules.'
}

foreach ($cron in $production.crons) {
    if (-not $expectedCrons.ContainsKey([string]$cron.path) -or
        $expectedCrons[[string]$cron.path] -ne [string]$cron.schedule) {
        throw "Unexpected production cron configuration for $($cron.path)."
    }
}

if ($null -ne $preview.crons -and @($preview.crons).Count -gt 0) {
    throw 'Preview Vercel configuration must not register production cron schedules.'
}

if ($production.git.deploymentEnabled.main -ne $true -or
    $preview.git.deploymentEnabled.main -ne $true -or
    $production.git.deploymentEnabled.'*' -ne $false -or
    $preview.git.deploymentEnabled.'*' -ne $false) {
    throw 'Production and Preview configurations must retain the reviewed Git deployment policy.'
}

$ignoredPaths = Get-Content -LiteralPath $ignorePath |
    ForEach-Object { $_.Trim() } |
    Where-Object { $_ -and -not $_.StartsWith('#') }
if ($ignoredPaths -contains 'vercel.json') {
    throw 'vercel.json must be uploaded or Vercel will silently omit Production cron definitions from container deployments.'
}

$environmentAuditPath = Join-Path $PSScriptRoot 'audit-vercel-environment-security.ps1'
$tokens = $null
$parseErrors = $null
[Management.Automation.Language.Parser]::ParseFile(
    $environmentAuditPath,
    [ref]$tokens,
    [ref]$parseErrors) | Out-Null
if ($parseErrors.Count -gt 0) {
    throw "Vercel environment-security audit contains parse errors: $($parseErrors.Message -join '; ')"
}
$environmentAudit = Get-Content -Raw -LiteralPath $environmentAuditPath
foreach ($requiredInvariant in @(
    'DATABASE_URL_UNPOOLED',
    'ApplicationSettings__JWT_Secret',
    'Security__EmailConfirmationSecret',
    'CRON_SECRET',
    'Email__Password',
    'OTEL_EXPORTER_OTLP_HEADERS',
    "type -ne 'sensitive'")) {
    if (-not $environmentAudit.Contains($requiredInvariant)) {
        throw "Vercel environment-security audit is missing invariant: $requiredInvariant"
    }
}

$observabilityAuditPath = Join-Path $PSScriptRoot 'audit-vercel-observability.ps1'
$tokens = $null
$parseErrors = $null
[Management.Automation.Language.Parser]::ParseFile(
    $observabilityAuditPath,
    [ref]$tokens,
    [ref]$parseErrors) | Out-Null
if ($parseErrors.Count -gt 0) {
    throw "Vercel observability audit contains parse errors: $($parseErrors.Message -join '; ')"
}
$observabilityAudit = Get-Content -Raw -LiteralPath $observabilityAuditPath
foreach ($requiredInvariant in @(
    "`$VercelCliVersion = '59.10.0'",
    'npx --yes "vercel@$VercelCliVersion"',
    'OTEL_EXPORTER_OTLP_ENDPOINT',
    'RequireExternalExport',
    'Get-MonitoringProductName')) {
    if (-not $observabilityAudit.Contains($requiredInvariant)) {
        throw "Vercel observability audit is missing invariant: $requiredInvariant"
    }
}

$cronAuditPath = Join-Path $PSScriptRoot 'audit-vercel-crons.ps1'
$tokens = $null
$parseErrors = $null
[Management.Automation.Language.Parser]::ParseFile(
    $cronAuditPath,
    [ref]$tokens,
    [ref]$parseErrors) | Out-Null
if ($parseErrors.Count -gt 0) {
    throw "Vercel cron audit contains parse errors: $($parseErrors.Message -join '; ')"
}
$cronAudit = Get-Content -Raw -LiteralPath $cronAuditPath
foreach ($requiredInvariant in @(
    "`$VercelCliVersion = '59.10.0'",
    '/api/maintenance/email-outbox',
    '/api/maintenance/cleanup',
    'cronsDisabledAt',
    'definitions',
    'deploymentId')) {
    if (-not $cronAudit.Contains($requiredInvariant)) {
        throw "Vercel cron audit is missing invariant: $requiredInvariant"
    }
}

Write-Host 'Vercel configuration verification passed.'
