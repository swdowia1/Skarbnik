---
name: analiza
description: "Analizuje problem i istniejący kod Skarbnik bez edycji plików. Użyj do diagnozy, oceny zależności, ryzyk i planu zmian."
---

# Analiza Skarbnik

Zawsze stosuj główny `AGENTS.md`; w razie rozbieżności nadrzędne są jego zasady.

## Przebieg

1. Przeanalizuj wymaganie, strukturę repozytorium i stan Git.
2. Odczytaj związane strony Razor, page modele, serwisy, kontrolery, modele, `AppDbContext` i testy.
3. Sprawdź analogiczne istniejące funkcje oraz dostępne polecenia budowania i testów.
4. Prześledź przepływ danych i zależności. W raporcie odróżnij potwierdzone fakty od hipotez.
5. Wskaż konkretny plan i listę plików, które mogłyby wymagać zmiany.

To zadanie jest tylko do odczytu. Nie edytuj ani nie twórz plików, nie uruchamiaj aplikacji, migracji ani operacji na bazie. Nie odczytuj ani nie ujawniaj produkcyjnych connection stringów lub sekretów.

Nie łącz się z produkcyjną bazą i nie zmieniaj jej konfiguracji. Jeśli w ramach zadania uruchamiasz weryfikację, ogranicz ją do najwyżej 3 pełnych prób łącznie; po trzecim niepowodzeniu przerwij i opisz problem. Nie wykonuj commitów ani push bez wyraźnej zgody użytkownika.

Raportuj ustalenia, konkretne pliki i metody, ryzyka oraz ograniczenia analizy.
