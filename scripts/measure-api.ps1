param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern('^https://')]
    [string]$BaseUrl,
    [ValidateRange(1, 10000)] [int]$Iterations = 100,
    [ValidateRange(1, 32)] [int]$Concurrency = 4,
    [string[]]$Paths = @('/health/live', '/api/Client/UserList?PageNumber=0&PageSize=20'),
    [string]$ScenariosFile,
    [string]$DatasetManifest,
    [string]$OutputJson,
    [string]$BearerTokenEnvironmentVariable = 'PERFORMANCE_BEARER_TOKEN',
    [string]$OwnerEmailEnvironmentVariable = 'PERFORMANCE_OWNER_EMAIL',
    [string]$OwnerPasswordEnvironmentVariable = 'PERFORMANCE_OWNER_PASSWORD',
    [string]$TrustedOriginEnvironmentVariable = 'PERFORMANCE_TRUSTED_ORIGIN',
    [string]$ProtectionBypassEnvironmentVariable = 'VERCEL_AUTOMATION_BYPASS_SECRET',
    [switch]$SkipWarmup
)

$ErrorActionPreference = 'Stop'
if ($PSVersionTable.PSVersion.Major -lt 7) {
    throw 'measure-api.ps1 requires PowerShell 7 or later (pwsh) for parallel requests and HTTP status inspection.'
}
$allowedMethods = @('GET', 'POST', 'PUT', 'PATCH', 'DELETE')
$dataset = $null
if ($DatasetManifest) {
    if (-not (Test-Path -LiteralPath $DatasetManifest -PathType Leaf)) {
        throw "Dataset manifest was not found: $DatasetManifest"
    }
    $dataset = Get-Content -Raw -LiteralPath $DatasetManifest | ConvertFrom-Json
    $requiredCounts = @{
        confirmedUsers = 10000; ownerProjects = 20; ownerExperiences = 10
        ownerEducations = 10; ownerCertificates = 20; ownerSkills = 30; ownerContactMessages = 100
    }
    foreach ($entry in $requiredCounts.GetEnumerator()) {
        if ([int]$dataset.counts.($entry.Key) -lt $entry.Value) {
            throw "Dataset manifest requires at least $($entry.Value) $($entry.Key); found $($dataset.counts.($entry.Key))."
        }
    }
}

if ($ScenariosFile) {
    if (-not (Test-Path -LiteralPath $ScenariosFile -PathType Leaf)) {
        throw "Scenario file was not found: $ScenariosFile"
    }
    $scenarioInput = @(Get-Content -Raw -LiteralPath $ScenariosFile | ConvertFrom-Json)
}
else {
    $scenarioInput = @($Paths | ForEach-Object {
        [pscustomobject]@{ Name = $_; Path = $_; Method = 'GET'; Authenticated = $false }
    })
}

$scenarios = foreach ($scenario in $scenarioInput) {
    $method = ([string]$scenario.Method).ToUpperInvariant()
    if ($allowedMethods -notcontains $method) {
        throw "Scenario '$($scenario.Name)' uses unsupported method '$method'."
    }
    if ([string]::IsNullOrWhiteSpace($scenario.Path) -or -not ([string]$scenario.Path).StartsWith('/')) {
        throw "Scenario '$($scenario.Name)' must use a root-relative path."
    }

    $body = if ($null -eq $scenario.Body) {
        $null
    }
    elseif ($scenario.Body -is [string]) {
        [string]$scenario.Body
    }
    else {
        $scenario.Body | ConvertTo-Json -Depth 20 -Compress
    }

    $resolvedPath = [string]$scenario.Path
    if ($null -ne $dataset) {
        $resolvedPath = $resolvedPath.Replace('{{ownerUsername}}', [uri]::EscapeDataString([string]$dataset.ownerUsername))
    }
    elseif ($resolvedPath.Contains('{{ownerUsername}}')) {
        throw "Scenario '$($scenario.Name)' requires -DatasetManifest to resolve ownerUsername."
    }

    [pscustomobject]@{
        Name = if ([string]::IsNullOrWhiteSpace($scenario.Name)) { "$method $($scenario.Path)" } else { [string]$scenario.Name }
        Path = $resolvedPath
        Method = $method
        Authenticated = [bool]$scenario.Authenticated
        Body = $body
        ExpectedStatusCodes = if ($null -eq $scenario.ExpectedStatusCodes) {
            @()
        }
        else {
            @($scenario.ExpectedStatusCodes | ForEach-Object { [int]$_ })
        }
        Iterations = if ($null -eq $scenario.Iterations) { $Iterations } else { [int]$scenario.Iterations }
    }
}
if ($scenarios.Count -eq 0) {
    throw 'At least one performance scenario is required.'
}
$duplicateNames = @($scenarios | Group-Object Name | Where-Object Count -gt 1)
if ($duplicateNames.Count -gt 0) {
    throw "Performance scenario names must be unique: $($duplicateNames.Name -join ', ')"
}

$bearerToken = [Environment]::GetEnvironmentVariable($BearerTokenEnvironmentVariable)
$protectionBypass = [Environment]::GetEnvironmentVariable($ProtectionBypassEnvironmentVariable)
$authCookie = $null
$trustedOrigin = [Environment]::GetEnvironmentVariable($TrustedOriginEnvironmentVariable)
if ($scenarios.Authenticated -contains $true -and [string]::IsNullOrWhiteSpace($bearerToken)) {
    $ownerEmail = [Environment]::GetEnvironmentVariable($OwnerEmailEnvironmentVariable)
    $ownerPassword = [Environment]::GetEnvironmentVariable($OwnerPasswordEnvironmentVariable)
    if ([string]::IsNullOrWhiteSpace($ownerEmail) -or [string]::IsNullOrWhiteSpace($ownerPassword)) {
        throw "Authenticated scenarios require either $BearerTokenEnvironmentVariable or both $OwnerEmailEnvironmentVariable and $OwnerPasswordEnvironmentVariable."
    }
    $loginHeaders = @{}
    if (-not [string]::IsNullOrWhiteSpace($protectionBypass)) {
        $loginHeaders['x-vercel-protection-bypass'] = $protectionBypass
    }
    $loginUri = '{0}/api/Account/Login' -f $BaseUrl.TrimEnd('/')
    $loginBody = @{ email = $ownerEmail; password = $ownerPassword; rememberMe = $false } | ConvertTo-Json -Compress
    $login = Invoke-WebRequest -Uri $loginUri -Method Post -Headers $loginHeaders -Body $loginBody `
        -ContentType 'application/json' -SessionVariable ownerSession -SkipHttpErrorCheck -TimeoutSec 30
    if ([int]$login.StatusCode -ne 200) { throw "Performance-owner login returned HTTP $([int]$login.StatusCode)." }
    $authCookie = $ownerSession.Cookies.GetCookieHeader([uri]$BaseUrl)
    if ([string]::IsNullOrWhiteSpace($authCookie)) { throw 'Performance-owner login did not issue an authentication cookie.' }
}
$hasCookieMutation = $scenarios | Where-Object { $_.Authenticated -and $_.Method -notin @('GET', 'HEAD', 'OPTIONS') }
if ($hasCookieMutation -and -not $bearerToken -and [string]::IsNullOrWhiteSpace($trustedOrigin)) {
    throw "Cookie-authenticated mutations require $TrustedOriginEnvironmentVariable to match an allowed frontend origin."
}
$results = [System.Collections.Concurrent.ConcurrentBag[object]]::new()

function New-RequestParameters([object]$Scenario, [string]$Uri) {
    $headers = @{}
    if ($Scenario.Authenticated -and $bearerToken) { $headers.Authorization = "Bearer $bearerToken" }
    if ($Scenario.Authenticated -and $authCookie) { $headers.Cookie = $authCookie }
    if ($Scenario.Authenticated -and $authCookie -and $Scenario.Method -notin @('GET', 'HEAD', 'OPTIONS')) { $headers.Origin = $trustedOrigin }
    if (-not [string]::IsNullOrWhiteSpace($protectionBypass)) {
        $headers['x-vercel-protection-bypass'] = $protectionBypass
    }
    $parameters = @{
        Uri = $Uri; Method = $Scenario.Method; Headers = $headers
        TimeoutSec = 30; SkipHttpErrorCheck = $true
    }
    if ($null -ne $Scenario.Body) {
        $parameters.Body = $Scenario.Body
        $parameters.ContentType = 'application/json'
    }
    return $parameters
}

foreach ($scenario in $scenarios) {
    if ($scenario.Iterations -lt 1 -or $scenario.Iterations -gt 10000) {
        throw "Scenario '$($scenario.Name)' iterations must be between 1 and 10000."
    }
    $uri = '{0}/{1}' -f $BaseUrl.TrimEnd('/'), $scenario.Path.TrimStart('/')
    if (-not $SkipWarmup) {
        $warmupParameters = New-RequestParameters $scenario $uri
        $warmup = Invoke-WebRequest @warmupParameters
        $warmupStatus = [int]$warmup.StatusCode
        $warmupFailed = if ($scenario.ExpectedStatusCodes.Count -gt 0) {
            $scenario.ExpectedStatusCodes -notcontains $warmupStatus
        }
        else {
            $warmupStatus -lt 200 -or $warmupStatus -ge 400
        }
        if ($warmupFailed) {
            throw "Warmup for '$($scenario.Name)' returned HTTP $([int]$warmup.StatusCode)."
        }
    }

    1..$scenario.Iterations | ForEach-Object -Parallel {
        $resultBag = $using:results
        $currentScenario = $using:scenario
        $currentBearer = $using:bearerToken
        $currentCookie = $using:authCookie
        $currentOrigin = $using:trustedOrigin
        $currentBypass = $using:protectionBypass
        $startedAt = [System.Diagnostics.Stopwatch]::StartNew()
        $statusCode = 0
        $applicationDuration = $null
        $cacheStatus = $null
        $cacheAgeSeconds = $null
        try {
            $headers = @{}
            if ($currentScenario.Authenticated -and $currentBearer) { $headers.Authorization = "Bearer $currentBearer" }
            if ($currentScenario.Authenticated -and $currentCookie) { $headers.Cookie = $currentCookie }
            if ($currentScenario.Authenticated -and $currentCookie -and $currentScenario.Method -notin @('GET', 'HEAD', 'OPTIONS')) { $headers.Origin = $currentOrigin }
            if (-not [string]::IsNullOrWhiteSpace($currentBypass)) {
                $headers['x-vercel-protection-bypass'] = $currentBypass
            }
            $parameters = @{
                Uri = $using:uri; Method = $currentScenario.Method; Headers = $headers
                TimeoutSec = 30; SkipHttpErrorCheck = $true
            }
            if ($null -ne $currentScenario.Body) {
                $parameters.Body = $currentScenario.Body
                $parameters.ContentType = 'application/json'
            }
            $response = Invoke-WebRequest @parameters
            $statusCode = [int]$response.StatusCode
            $cacheStatusHeader = [string]::Join(',', @($response.Headers['X-Vercel-Cache']))
            if (-not [string]::IsNullOrWhiteSpace($cacheStatusHeader)) {
                $cacheStatus = $cacheStatusHeader.Trim().ToUpperInvariant()
            }
            $ageHeader = [string]::Join(',', @($response.Headers['Age']))
            $parsedAge = 0
            if ([int]::TryParse(
                $ageHeader,
                [System.Globalization.NumberStyles]::Integer,
                [System.Globalization.CultureInfo]::InvariantCulture,
                [ref]$parsedAge)) {
                $cacheAgeSeconds = $parsedAge
            }
            $serverTiming = [string]::Join(',', @($response.Headers['Server-Timing']))
            if ($serverTiming -match '(?:^|,)\s*app;dur=([0-9]+(?:\.[0-9]+)?)') {
                $applicationDuration = [double]::Parse($matches[1], [System.Globalization.CultureInfo]::InvariantCulture)
            }
        }
        catch { $statusCode = -1 }
        finally {
            $startedAt.Stop()
            $wallDuration = $startedAt.Elapsed.TotalMilliseconds
            $isCacheHit = $cacheStatus -in @('HIT', 'STALE') -or
                ($null -ne $cacheAgeSeconds -and $cacheAgeSeconds -gt 0)
            $discardedApplicationTiming = $null -ne $applicationDuration -and
                ($isCacheHit -or $applicationDuration -gt ($wallDuration + 5))
            if ($discardedApplicationTiming) {
                # Server-Timing is generated at the origin. A shared cache can replay
                # that header on a later, faster response, so it is not a measurement
                # of this request. The small tolerance covers stopwatch/header rounding.
                $applicationDuration = $null
            }
            $resultBag.Add([pscustomobject]@{
                Name = $currentScenario.Name; StatusCode = $statusCode
                DurationMs = $wallDuration
                ApplicationDurationMs = $applicationDuration
                ApplicationTimingDiscarded = $discardedApplicationTiming
                CacheStatus = $cacheStatus
                CacheAgeSeconds = $cacheAgeSeconds
            })
        }
    } -ThrottleLimit $Concurrency
}

function Get-Percentile([double[]]$Values, [double]$Percentile) {
    if ($Values.Count -eq 0) { return $null }
    $index = [math]::Max(0, [math]::Ceiling($Values.Count * $Percentile) - 1)
    return [math]::Round($Values[$index], 1)
}

$summary = foreach ($scenario in $scenarios) {
    $samples = @($results | Where-Object Name -eq $scenario.Name | Sort-Object DurationMs)
    $durations = [double[]]@($samples.DurationMs)
    $applicationDurations = [double[]]@($samples |
        Where-Object { $null -ne $_.ApplicationDurationMs } |
        Sort-Object ApplicationDurationMs |
        Select-Object -ExpandProperty ApplicationDurationMs)
    $unexpected = if ($scenario.ExpectedStatusCodes.Count -gt 0) {
        @($samples | Where-Object { $scenario.ExpectedStatusCodes -notcontains $_.StatusCode }).Count
    }
    else {
        @($samples | Where-Object { $_.StatusCode -lt 200 -or $_.StatusCode -ge 400 }).Count
    }
    [pscustomobject]@{
        Scenario = $scenario.Name; Requests = $durations.Count; Errors = $unexpected
        WallP50Ms = Get-Percentile $durations 0.50
        WallP95Ms = Get-Percentile $durations 0.95
        WallP99Ms = Get-Percentile $durations 0.99
        AppP95Ms = Get-Percentile $applicationDurations 0.95
        AppTimingSamples = $applicationDurations.Count
        DiscardedAppTimingSamples = @($samples | Where-Object ApplicationTimingDiscarded).Count
        CacheHits = @($samples | Where-Object {
            $_.CacheStatus -in @('HIT', 'STALE') -or
            ($null -ne $_.CacheAgeSeconds -and $_.CacheAgeSeconds -gt 0)
        }).Count
        MaxMs = if ($durations.Count -eq 0) { $null } else { [math]::Round($durations[-1], 1) }
    }
}

$summary | Format-Table -AutoSize
if (($summary | Measure-Object Errors -Sum).Sum -gt 0) {
    throw 'Performance measurement observed unexpected HTTP responses.'
}
if ($OutputJson) {
    $evidence = [ordered]@{
        measuredAtUtc = [DateTime]::UtcNow.ToString('O')
        baseUrl = $BaseUrl
        iterationsDefault = $Iterations
        concurrency = $Concurrency
        dataset = $dataset
        results = @($summary)
    }
    $fullOutputPath = [IO.Path]::GetFullPath($OutputJson)
    $parent = [IO.Path]::GetDirectoryName($fullOutputPath)
    if (-not [string]::IsNullOrWhiteSpace($parent)) { [IO.Directory]::CreateDirectory($parent) | Out-Null }
    [IO.File]::WriteAllText($fullOutputPath, ($evidence | ConvertTo-Json -Depth 20), [Text.UTF8Encoding]::new($false))
    Write-Host "Evidence: $fullOutputPath"
}
