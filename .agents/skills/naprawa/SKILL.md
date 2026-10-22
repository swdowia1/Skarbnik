---
name: naprawa
description: "Diagnozuje i naprawia usterki Skarbnik minimalną zmianą, aktualizuje testy i uruchamia scripts/verify.ps1."
---

# Naprawa Skarbnik

Zawsze stosuj główny `AGENTS.md`. Zachowaj architekturę ASP.NET Core Razor Pages, .NET 7 i EF Core 7.0.20, chyba że użytkownik zatwierdzi zmianę.

## Przed edycją

1. Odczytaj powiązany kod, testy i komunikaty błędów.
2. Ustal przyczynę na podstawie dostępnych dowodów.
3. Przedstaw plan oraz listę plików do zmiany.
4. Zachowaj niezapisane zmiany użytkownika.

## Implementacja

- Wprowadź minimalną poprawkę zgodną z istniejącym stylem.
- Dodaj lub zaktualizuj wartościowe testy w `Skarbnik.Tests`.
- Nie usuwaj testów ani nie osłabiaj asercji, by uzyskać zielony wynik.
- Nie naprawiaj problemów niezwiązanych z zadaniem.

## Weryfikacja i bezpieczeństwo

Po zmianach uruchom `scripts/verify.ps1` z katalogu głównego. Analizuj błędy; poprawiaj wyłącznie własne zmiany i ponawiaj weryfikację najwyżej 3 razy łącznie. Po trzecim niepowodzeniu przerwij i opisz problem.

Nie łącz się z produkcyjną bazą, nie uruchamiaj migracji ani nie zmieniaj produkcyjnej konfiguracji. Testy EF Core muszą używać izolowanej bazy testowej, np. SQLite w pamięci.

Nie wykonuj commitów ani push bez wyraźnej zgody użytkownika.

## Raport

Podaj zmienione pliki, testy, wynik weryfikacji, liczbę prób, ostrzeżenia i nierozwiązane problemy. Nie deklaruj sukcesu dla etapów, które nie przeszły albo nie zostały uruchomione.
