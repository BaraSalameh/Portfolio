$ErrorActionPreference = 'Stop'
$configurationPath = Join-Path $PSScriptRoot '..' '.github' 'dependabot.yml'
if (-not (Test-Path -LiteralPath $configurationPath -PathType Leaf)) {
    throw 'Dependabot configuration is required.'
}

$configuration = Get-Content -Raw -LiteralPath $configurationPath
foreach ($ecosystem in @('nuget', 'github-actions', 'docker')) {
    if ($configuration -notmatch "package-ecosystem:\s*$([regex]::Escape($ecosystem))") {
        throw "Dependabot must cover the $ecosystem package ecosystem."
    }
}

$weeklySchedules = [regex]::Matches($configuration, 'interval:\s*weekly').Count
if ($weeklySchedules -ne 3) {
    throw 'Every dependency ecosystem must use a weekly servicing schedule.'
}
if ([regex]::Matches($configuration, 'open-pull-requests-limit:\s*[1-9]\d*').Count -ne 3) {
    throw 'Every dependency ecosystem must have a bounded positive pull-request limit.'
}
if ([regex]::Matches($configuration, 'update-types:\s*(?:\r?\n\s+-\s+(?:patch|minor)){2}').Count -ne 3) {
    throw 'Servicing groups must contain minor and patch updates; major upgrades require separate review.'
}

Write-Host 'Dependency automation verification passed.'
