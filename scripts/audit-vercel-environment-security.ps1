param(
    [string]$ProjectId = 'prj_BHybtEdd5lbCF89LCPoxuucmmuWw',
    [string]$TeamId = 'team_AoG1xWJRQcGGy84KpkGVuRgB',
    [string]$VercelCliVersion = '59.10.0'
)

$ErrorActionPreference = 'Stop'
$endpoint = "/v9/projects/$ProjectId/env?teamId=$TeamId"
$raw = & npx --yes "vercel@$VercelCliVersion" api $endpoint --raw 2>$null | Out-String
if ($LASTEXITCODE -ne 0) {
    throw 'Vercel environment metadata request failed.'
}

try {
    $response = $raw | ConvertFrom-Json
}
catch {
    throw 'Vercel environment metadata was not valid JSON.'
}

$environmentVariables = @($response.envs)
$requiredSensitiveKeys = @(
    'DATABASE_URL',
    'DATABASE_URL_UNPOOLED',
    'ApplicationSettings__JWT_Secret',
    'Security__EmailConfirmationSecret',
    'CRON_SECRET',
    'Email__Password'
)
$optionalSensitiveKeys = @(
    'Security__PreviousEmailConfirmationSecret',
    'OTEL_EXPORTER_OTLP_HEADERS'
)
$failures = [Collections.Generic.List[string]]::new()

foreach ($environment in @('preview', 'production')) {
    foreach ($key in $requiredSensitiveKeys) {
        $matches = @($environmentVariables | Where-Object {
            $_.key -eq $key -and @($_.target) -contains $environment
        })
        if ($matches.Count -ne 1) {
            $failures.Add("$environment requires exactly one $key variable; found $($matches.Count).")
            continue
        }
        if ($matches[0].type -ne 'sensitive') {
            $failures.Add("$environment $key must be Vercel Sensitive; found $($matches[0].type).")
        }
    }

    foreach ($key in $optionalSensitiveKeys) {
        $matches = @($environmentVariables | Where-Object {
            $_.key -eq $key -and @($_.target) -contains $environment
        })
        if ($matches.Count -gt 1) {
            $failures.Add("$environment has duplicate $key variables.")
        }
        elseif ($matches.Count -eq 1 -and $matches[0].type -ne 'sensitive') {
            $failures.Add("$environment $key must be Vercel Sensitive when configured; found $($matches[0].type).")
        }
    }
}

if ($failures.Count -gt 0) {
    $failures | ForEach-Object { Write-Error $_ -ErrorAction Continue }
    throw "Vercel environment security audit failed with $($failures.Count) metadata finding(s)."
}

Write-Host 'Vercel environment security audit passed without retrieving secret values.'
