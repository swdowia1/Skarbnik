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

$solutionProjects = @(
    foreach ($line in Get-Content -LiteralPath $solution) {
        if ($line -match '^Project\("[^"]+"\)\s*=\s*"[^"]+",\s*"([^"]+\.csproj)"\s*,') {
            $projectPath = Join-Path $root $Matches[1]
            if (Test-Path -LiteralPath $projectPath -PathType Leaf) {
                Get-Item -LiteralPath $projectPath
            }
        }
    }
)

$testProjects = @(
    foreach ($project in $solutionProjects) {
        [xml]$projectDocument = Get-Content -LiteralPath $project.FullName -Raw
        $isTestProject = $projectDocument.SelectNodes("//*[local-name()='IsTestProject']") |
            Where-Object { $_.InnerText.Trim() -eq 'true' } |
            Select-Object -First 1

        if ($null -ne $isTestProject) {
            $project
        }
    }
)

if ($testProjects.Count -eq 0) {
    throw "No test projects found in Skarbnik.sln. Mark a solution project with <IsTestProject>true</IsTestProject>."
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

    Invoke-RequiredCommand -Name "GIT DIFF CHECK (UNSTAGED)" -FilePath "git" -Arguments @(
        "diff", "--check"
    )

    Invoke-RequiredCommand -Name "GIT DIFF CHECK (STAGED)" -FilePath "git" -Arguments @(
        "diff", "--cached", "--check"
    )

    Write-Host "Untracked files are not checked by git diff --check." -ForegroundColor Yellow

    Write-Host "`nVerification completed successfully." -ForegroundColor Green
}
finally {
    Pop-Location
}
