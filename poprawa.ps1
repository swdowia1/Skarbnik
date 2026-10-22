$ErrorActionPreference = "Stop"

$plik = Join-Path $PSScriptRoot ".agents\skills\funkcja\SKILL.md"

if (!(Test-Path $plik)) {
    throw "Nie znaleziono pliku: $plik"
}

$tresc = [System.IO.File]::ReadAllText($plik)

$separator = [regex]::Match(
    $tresc,
    '(?m)^---\r?\n'
)

if (!$separator.Success) {
    throw "Nie znaleziono pocz¹tku nag³ówka YAML."
}

$koniecNaglowka = [regex]::Match(
    $tresc.Substring($separator.Length),
    '(?m)^---\r?\n'
)

if (!$koniecNaglowka.Success) {
    throw "Nie znaleziono koñca nag³ówka YAML."
}

$koniec = $separator.Length + $koniecNaglowka.Index + $koniecNaglowka.Length

$naglowek = @'
---
name: funkcja
description: "Implementuje nowe funkcjonalnoœci w projekcie SkarbnikCodex. Analizuje kod, przedstawia plan, wprowadza zmiany, dodaje testy i uruchamia weryfikacjê."
---
'@

$nowaTresc = $naglowek + "`r`n" + $tresc.Substring($koniec).TrimStart("`r", "`n")

$kopia = "$plik.bak-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
Copy-Item -LiteralPath $plik -Destination $kopia

$utf8 = [System.Text.UTF8Encoding]::new($false)
[System.IO.File]::WriteAllText($plik, $nowaTresc, $utf8)

Write-Host "Poprawiono nag³ówek skilla funkcja." -ForegroundColor Green
Write-Host "Kopia zapasowa: $kopia"
Write-Host "`nNowy nag³ówek:"
Get-Content -LiteralPath $plik -TotalCount 4
