param(
    [string]$Scope = 'albaraa-salamas-projects',
    [string]$Project = 'portfolio-api',
    [string]$TeamId = 'team_AoG1xWJRQcGGy84KpkGVuRgB',
    [switch]$RequireExternalExport
)

$ErrorActionPreference = 'Stop'
$VercelCliVersion = '59.10.0'

function Invoke-VercelJson {
    param([string[]]$Arguments)

    $output = & npx --yes "vercel@$VercelCliVersion" @Arguments 2>$null
    if ($LASTEXITCODE -ne 0) {
        throw "Vercel CLI request failed: vercel $($Arguments -join ' ')"
    }

    return $output | ConvertFrom-Json
}

function Get-MonitoringProductName {
    param([object]$Resource)

    if ($null -ne $Resource.product) {
        if ($null -ne $Resource.product.name) {
            return [string]$Resource.product.name
        }

        return [string]$Resource.product
    }

    if ($null -ne $Resource.integration) {
        if ($null -ne $Resource.integration.name) {
            return [string]$Resource.integration.name
        }

        return [string]$Resource.integration
    }

    return ''
}

$productionEnvironment = Invoke-VercelJson @(
    'env', 'ls', 'production', '--json',
    '--project', $Project,
    '--scope', $Scope)
$environmentNames = @($productionEnvironment.envs | ForEach-Object { [string]$_.key })
$hasOtlpEndpoint = $environmentNames -contains 'OTEL_EXPORTER_OTLP_ENDPOINT'
$hasOtlpHeaders = $environmentNames -contains 'OTEL_EXPORTER_OTLP_HEADERS'

$drainResponse = Invoke-VercelJson @(
    'api', "/v1/drains?teamId=$TeamId", '--raw', '--scope', $Scope)
$drains = if ($null -ne $drainResponse.drains) {
    @($drainResponse.drains)
} else {
    @($drainResponse)
}

$integrationResponse = Invoke-VercelJson @(
    'integration', 'list', $Project, '--json', '--scope', $Scope)
$resources = if ($null -ne $integrationResponse.resources) {
    @($integrationResponse.resources)
} else {
    @($integrationResponse)
}
$monitoringProducts = @(
    'Axiom', 'Better Stack', 'Checkly', 'Datadog', 'Grafana', 'Honeycomb',
    'Logtail', 'New Relic', 'Sentry'
)
$monitoringResources = @($resources | Where-Object {
    $product = Get-MonitoringProductName $_
    $monitoringProducts -contains $product
})

Write-Host "Production OTLP endpoint configured: $hasOtlpEndpoint"
Write-Host "Production OTLP headers configured: $hasOtlpHeaders"
Write-Host "Configured Vercel drains: $($drains.Count)"
Write-Host "Project monitoring resources: $($monitoringResources.Count)"

$hasExternalExport = $hasOtlpEndpoint -or $drains.Count -gt 0 -or $monitoringResources.Count -gt 0
if ($RequireExternalExport -and -not $hasExternalExport) {
    throw 'Production has application telemetry instrumentation but no external OTLP endpoint, Vercel drain, or project monitoring resource.'
}

if (-not $hasExternalExport) {
    Write-Warning 'Observability is limited to Vercel runtime logs and manual CLI/Dashboard inspection.'
}
