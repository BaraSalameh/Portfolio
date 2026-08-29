param(
    [string]$Scope = 'albaraa-salamas-projects',
    [string]$Project = 'portfolio-api',
    [string]$TeamId = 'team_AoG1xWJRQcGGy84KpkGVuRgB'
)

$ErrorActionPreference = 'Stop'
$VercelCliVersion = '59.10.0'
$expected = @{
    '/api/maintenance/email-outbox' = '17 7 * * *'
    '/api/maintenance/cleanup' = '17 3 * * *'
}

$raw = & npx --yes "vercel@$VercelCliVersion" api `
    "/v9/projects/$Project`?teamId=$TeamId" --raw --scope $Scope 2>$null
if ($LASTEXITCODE -ne 0) {
    throw 'Unable to retrieve Vercel project cron metadata.'
}

$projectMetadata = $raw | ConvertFrom-Json
if ($null -ne $projectMetadata.cronsDisabledAt -or
    $null -ne $projectMetadata.crons.disabledAt) {
    throw 'Vercel cron execution is disabled for the Production project.'
}

$definitions = @($projectMetadata.crons.definitions)
if ($definitions.Count -ne $expected.Count) {
    throw "Expected $($expected.Count) Production cron definitions but found $($definitions.Count)."
}

foreach ($definition in $definitions) {
    $path = [string]$definition.path
    $schedule = [string]$definition.schedule
    if (-not $expected.ContainsKey($path) -or $expected[$path] -ne $schedule) {
        throw "Unexpected Production cron definition: $path ($schedule)."
    }
    if ([string]::IsNullOrWhiteSpace([string]$definition.host)) {
        throw "Production cron definition has no deployment host: $path."
    }
}

if ([string]::IsNullOrWhiteSpace([string]$projectMetadata.crons.deploymentId)) {
    throw 'Production cron definitions are not associated with a deployment.'
}

Write-Host "Vercel cron audit passed for deployment $($projectMetadata.crons.deploymentId)."
