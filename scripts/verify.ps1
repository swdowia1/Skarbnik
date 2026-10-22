$ErrorActionPreference = "Stop"

Write-Host "=== SKARBNIK: PROJECT VERIFICATION ===" -ForegroundColor Cyan

$root = Split-Path -Parent $PSScriptRoot

$project = Get-ChildItem -Path $root -Filter "*.csproj" -File -Recurse |
    Where-Object {
        $_.FullName -notmatch '[\\/](bin|obj)[\\/]'
    } |
    Select-Object -First 1

if (-not $project) {
    throw "Project file not found."
}

Write-Host "`nProject: $($project.FullName)" -ForegroundColor Yellow

Push-Location $root

try {
    Write-Host "`n=== RESTORE ===" -ForegroundColor Cyan

    dotnet restore $project.FullName

    if ($LASTEXITCODE -ne 0) {
        throw "Restore failed."
    }

    Write-Host "`n=== BUILD ===" -ForegroundColor Cyan

    dotnet build $project.FullName --no-restore

    if ($LASTEXITCODE -ne 0) {
        throw "Build failed."
    }

    Write-Host "`n=== TESTS ===" -ForegroundColor Cyan

    $testProjects = Get-ChildItem -Path $root -Filter "*.csproj" -File -Recurse |
        Where-Object {
            $_.Name -match 'Tests?\.csproj$' -and
            $_.FullName -notmatch '[\\/](bin|obj)[\\/]'
        }

    if (-not $testProjects) {
        Write-Host "No test projects found."
    }
    else {
        foreach ($testProject in $testProjects) {
            Write-Host "Testing: $($testProject.Name)"

            dotnet test $testProject.FullName --no-restore

            if ($LASTEXITCODE -ne 0) {
                throw "Tests failed: $($testProject.Name)"
            }
        }
    }

    Write-Host "`n=== GIT DIFF CHECK ===" -ForegroundColor Cyan

    git diff --check

    if ($LASTEXITCODE -ne 0) {
        throw "Git diff check failed."
    }

    Write-Host "`nVerification completed." -ForegroundColor Green
}
finally {
    Pop-Location
}