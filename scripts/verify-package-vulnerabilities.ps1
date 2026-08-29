param(
    [ValidateSet('Low', 'Moderate', 'High', 'Critical')]
    [string]$MinimumSeverity = 'High'
)

$ErrorActionPreference = 'Stop'
$severityRank = @{
    Low = 1
    Moderate = 2
    High = 3
    Critical = 4
}

$json = dotnet list Portfolio.sln package --vulnerable --include-transitive --format json 2>&1 | Out-String
if ($LASTEXITCODE -ne 0) {
    throw "NuGet vulnerability scan failed with exit code $LASTEXITCODE.`n$json"
}

try {
    $report = $json | ConvertFrom-Json -Depth 20
}
catch {
    throw "NuGet vulnerability scan did not return valid JSON.`n$json"
}

$findings = foreach ($project in @($report.projects)) {
    foreach ($framework in @($project.frameworks)) {
        foreach ($collectionName in @('topLevelPackages', 'transitivePackages')) {
            foreach ($package in @($framework.$collectionName)) {
                if ($null -eq $package) {
                    continue
                }

                foreach ($vulnerability in @($package.vulnerabilities)) {
                    if ($null -eq $vulnerability) {
                        continue
                    }

                    [pscustomobject]@{
                        Project = $project.path
                        Framework = $framework.framework
                        Package = $package.id
                        Version = $package.resolvedVersion
                        Severity = $vulnerability.severity
                        Advisory = $vulnerability.advisoryurl
                    }
                }
            }
        }
    }
}

$minimumRank = $severityRank[$MinimumSeverity]
$blockingFindings = @($findings | Where-Object {
    $null -ne $_ -and
    $null -ne $_.Severity -and
    $severityRank.ContainsKey([string]$_.Severity) -and
    $severityRank[[string]$_.Severity] -ge $minimumRank
})

if ($blockingFindings.Count -gt 0) {
    $blockingFindings | Format-Table Project, Package, Version, Severity, Advisory -AutoSize | Out-String | Write-Host
    throw "$($blockingFindings.Count) package vulnerability finding(s) met or exceeded the $MinimumSeverity severity gate."
}

Write-Host "Package vulnerability gate passed: no $MinimumSeverity or Critical findings."
