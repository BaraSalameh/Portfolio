param(
    [string]$BaseUrl,
    [string]$ProtectionBypassEnvironmentVariable = 'VERCEL_AUTOMATION_BYPASS_SECRET',
    [string]$VercelCliVersion = '59.10.0',
    [switch]$UseVercelCli,
    [switch]$ListChecks
)

$ErrorActionPreference = 'Stop'
$checkNames = @(
    'liveness',
    'readiness',
    'health-alias',
    'legacy-public-read',
    'explicit-v1-public-read',
    'v1-shape-compatibility',
    'unsupported-v2',
    'validation-problem-details',
    'owner-authentication-challenge',
    'maintenance-credential-challenge'
)

if ($ListChecks) {
    $checkNames
    return
}
if ([string]::IsNullOrWhiteSpace($BaseUrl) -or $BaseUrl -notmatch '^https://') {
    throw 'BaseUrl must be an HTTPS URL.'
}

$base = $BaseUrl.TrimEnd('/')
$protectionBypass = [Environment]::GetEnvironmentVariable($ProtectionBypassEnvironmentVariable)
$correlationId = "preview-smoke-$([Guid]::NewGuid().ToString('N'))"

function Invoke-SmokeRequest([string]$Method, [string]$Path, [string]$Body = '') {
    if ($UseVercelCli) {
        $curlArguments = @(
            '--yes', "vercel@$VercelCliVersion", 'curl', $Path,
            '--deployment', $base, '--', '--include', '--silent',
            '--request', $Method,
            '--header', "X-Correlation-ID: $correlationId"
        )
        if (-not [string]::IsNullOrEmpty($Body)) {
            $curlArguments += @(
                '--header', 'Content-Type: application/json',
                '--data-binary', $Body
            )
        }

        $raw = (& npx @curlArguments 2>&1 | Out-String)
        if ($LASTEXITCODE -ne 0) {
            throw "Vercel CLI request failed for $Method $Path."
        }

        $match = [regex]::Match(
            $raw,
            '(?ms)^HTTP/\S+\s+(?<status>\d{3})[^\r\n]*\r?\n(?<headers>.*?)\r?\n\r?\n(?<body>.*)$')
        if (-not $match.Success) {
            throw "Vercel CLI returned an unrecognized HTTP response for $Method $Path."
        }

        $headers = [System.Collections.Generic.Dictionary[string, string]]::new(
            [StringComparer]::OrdinalIgnoreCase)
        foreach ($line in ($match.Groups['headers'].Value -split '\r?\n')) {
            $separator = $line.IndexOf(':')
            if ($separator -gt 0) {
                $headers[$line.Substring(0, $separator).Trim()] =
                    $line.Substring($separator + 1).Trim()
            }
        }

        return [pscustomobject]@{
            StatusCode = [int]$match.Groups['status'].Value
            Headers = $headers
            Content = $match.Groups['body'].Value.TrimEnd()
        }
    }

    $headers = @{ 'X-Correlation-ID' = $correlationId }
    if (-not [string]::IsNullOrWhiteSpace($protectionBypass)) {
        $headers['x-vercel-protection-bypass'] = $protectionBypass
    }
    $parameters = @{
        Uri = "$base$Path"
        Method = $Method
        Headers = $headers
        MaximumRedirection = 0
        SkipHttpErrorCheck = $true
    }
    if (-not [string]::IsNullOrEmpty($Body)) {
        $parameters.ContentType = 'application/json'
        $parameters.Body = $Body
    }
    $response = Invoke-WebRequest @parameters
    $content = if ($response.Content -is [byte[]]) {
        [System.Text.Encoding]::UTF8.GetString($response.Content)
    } else {
        [string]$response.Content
    }

    return [pscustomobject]@{
        StatusCode = [int]$response.StatusCode
        Headers = $response.Headers
        Content = $content
    }
}

function Assert-Status([object]$Response, [int]$Expected, [string]$Check) {
    if ([int]$Response.StatusCode -ne $Expected) {
        throw "$Check expected HTTP $Expected but received $([int]$Response.StatusCode)."
    }
}

function Assert-Header([object]$Response, [string]$Name, [string]$Pattern, [string]$Check) {
    $value = [string]$Response.Headers[$Name]
    if ($value -notmatch $Pattern) {
        throw "$Check expected header $Name to match '$Pattern' but received '$value'."
    }
}

function Assert-ProblemDetails([object]$Response, [int]$Status, [string]$Check) {
    Assert-Status $Response $Status $Check
    Assert-Header $Response 'Content-Type' '^application/problem\+json' $Check
    $problem = $Response.Content | ConvertFrom-Json -AsHashtable
    if ([int]$problem.status -ne $Status -or [string]::IsNullOrWhiteSpace([string]$problem.traceId)) {
        throw "$Check returned incomplete RFC 7807 content."
    }
}

function Get-JsonShape([object]$Value) {
    if ($Value -is [System.Collections.IDictionary]) {
        return '{' + (($Value.Keys | Sort-Object | ForEach-Object {
            "$_`:$(Get-JsonShape $Value[$_])"
        }) -join ',') + '}'
    }
    if ($Value -is [System.Collections.IEnumerable] -and $Value -isnot [string]) {
        $items = @($Value)
        if ($items.Count -eq 0) {
            return '[]'
        }
        return "[$(Get-JsonShape $items[0])]"
    }
    if ($null -eq $Value) { return 'null' }
    return $Value.GetType().Name
}

$live = Invoke-SmokeRequest GET '/health/live'
Assert-Status $live 200 'liveness'
Assert-Header $live 'Content-Type' '^application/json' 'liveness'
Assert-Header $live 'X-Correlation-ID' "^$([regex]::Escape($correlationId))$" 'liveness'
Assert-Header $live 'X-Content-Type-Options' '^nosniff$' 'liveness'
Assert-Header $live 'Cache-Control' 'no-store' 'liveness'
if (($live.Content | ConvertFrom-Json).status -ne 'healthy') { throw 'liveness returned an unhealthy body.' }

$ready = Invoke-SmokeRequest GET '/health/ready'
Assert-Status $ready 200 'readiness'
if (($ready.Content | ConvertFrom-Json).status -ne 'healthy') { throw 'readiness returned an unhealthy body.' }

$healthAlias = Invoke-SmokeRequest GET '/health'
Assert-Status $healthAlias 200 'health-alias'

$legacy = Invoke-SmokeRequest GET '/api/Client/UserList?PageSize=20'
Assert-Status $legacy 200 'legacy-public-read'
Assert-Header $legacy 'Content-Type' '^application/json' 'legacy-public-read'
Assert-Header $legacy 'Cache-Control' 'public.*max-age=60|max-age=60.*public' 'legacy-public-read'

$explicitV1 = Invoke-SmokeRequest GET '/api/v1/Client/UserList?PageSize=20'
Assert-Status $explicitV1 200 'explicit-v1-public-read'
$legacyShape = Get-JsonShape ($legacy.Content | ConvertFrom-Json -AsHashtable)
$v1Shape = Get-JsonShape ($explicitV1.Content | ConvertFrom-Json -AsHashtable)
if ($legacyShape -ne $v1Shape) { throw 'v1-shape-compatibility detected different JSON shapes.' }

$unsupported = Invoke-SmokeRequest GET '/api/v2/Client/UserList'
Assert-Status $unsupported 404 'unsupported-v2'

$validation = Invoke-SmokeRequest POST '/api/Account/Login' '{}'
Assert-ProblemDetails $validation 400 'validation-problem-details'

$owner = Invoke-SmokeRequest GET '/api/Owner/UserInfo'
Assert-ProblemDetails $owner 401 'owner-authentication-challenge'
Assert-Header $owner 'WWW-Authenticate' '^Bearer' 'owner-authentication-challenge'
Assert-Header $owner 'Cache-Control' 'no-store' 'owner-authentication-challenge'

$maintenance = Invoke-SmokeRequest GET '/api/maintenance/cleanup'
Assert-ProblemDetails $maintenance 401 'maintenance-credential-challenge'

Write-Host "Preview smoke acceptance passed: $($checkNames.Count) checks against $base."
