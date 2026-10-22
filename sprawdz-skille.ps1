
$ErrorActionPreference = "Stop"

foreach ($nazwa in @("funkcja", "naprawa")) {
    $plik = Join-Path $PSScriptRoot ".agents\skills\$nazwa\SKILL.md"

    Write-Host "`n===== $nazwa =====" -ForegroundColor Cyan

    if (!(Test-Path $plik)) {
        Write-Host "BRAK PLIKU: $plik" -ForegroundColor Red
        continue
    }

    $bajty = [System.IO.File]::ReadAllBytes($plik)

    Write-Host "Œcie¿ka: $plik"
    Write-Host "Rozmiar: $($bajty.Length) bajtów"

    if ($bajty.Length -gt 0) {
        $liczba = [Math]::Min(7, $bajty.Length - 1)
        Write-Host "Pierwsze bajty: $([BitConverter]::ToString($bajty[0..$liczba]))"
    }

    Write-Host "`nPierwsze 5 linii:"
    Get-Content -LiteralPath $plik -TotalCount 5

    Write-Host "`n--- Koniec $nazwa ---"
}

