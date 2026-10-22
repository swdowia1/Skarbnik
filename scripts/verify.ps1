$ErrorActionPreference = "Stop"

function Invoke-RequiredCommand {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Name,
        [Parameter(Mandatory = $true)]
        [string]$FilePath,
        [Parameter(Mandatory = $true)]
        [string[]]$Arguments
    )

    Write-Host "`n=== $Name ===" -ForegroundColor Cyan
    & $FilePath @Arguments
    $exitCode = $LASTEXITCODE

    if ($exitCode -ne 0) {
        throw "$Name failed with exit code $exitCode."
    }
}

$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root "Skarbnik.sln"

if (-not (Test-Path -LiteralPath $solution -PathType Leaf)) {
    throw "Solution file not found: $solution"
}

$testProjects = @(
    Get-ChildItem -Path $root -Filter "*.csproj" -File -Recurse |
        Where-Object {
            if ($_.FullName -match '[\\/](bin|obj)[\\/]') {
                return $false
            }

            $projectContent = Get-Content -LiteralPath $_.FullName -Raw
            return $projectContent -match '<IsTestProject>\s*true\s*</IsTestProject>'
        } |
        Sort-Object FullName
)

if ($testProjects.Count -eq 0) {
    throw "No test projects found. Mark test projects with <IsTestProject>true</IsTestProject>."
}

Push-Location $root
try {
    Invoke-RequiredCommand -Name "RESTORE SOLUTION" -FilePath "dotnet" -Arguments @(
        "restore", $solution
    )

    Invoke-RequiredCommand -Name "BUILD SOLUTION" -FilePath "dotnet" -Arguments @(
        "build", $solution, "--no-restore"
    )

    foreach ($testProject in $testProjects) {
        Invoke-RequiredCommand -Name "TEST $($testProject.Name)" -FilePath "dotnet" -Arguments @(
            "test", $testProject.FullName, "--no-build", "--no-restore"
        )
    }

    Invoke-RequiredCommand -Name "GIT DIFF CHECK" -FilePath "git" -Arguments @(
        "diff", "--check"
    )

    Write-Host "`nVerification completed successfully." -ForegroundColor Green
}
finally {
    Pop-Location
}
