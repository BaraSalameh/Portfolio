$ErrorActionPreference = 'Stop'
$baseline = 0
$output = dotnet build Portfolio.sln -c Release --no-restore 2>&1 | Out-String
$output | Write-Host

if ($LASTEXITCODE -ne 0) {
    throw "Release build failed with exit code $LASTEXITCODE."
}

$summary = [regex]::Matches($output, '(?m)^\s*(\d+) Warning\(s\)\s*$')
if ($summary.Count -eq 0) {
    throw 'Could not determine the compiler warning count.'
}

$warningCount = [int]$summary[$summary.Count - 1].Groups[1].Value
if ($warningCount -gt $baseline) {
    throw "Compiler warnings increased from the baseline of $baseline to $warningCount."
}

Write-Host "Warning baseline passed: $warningCount/$baseline."
