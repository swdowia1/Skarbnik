---
name: testy
description: "Ocenia pokrycie testami Skarbnik i dodaje wartościowe testy w Skarbnik.Tests, bez dostępu do produkcyjnej bazy."
---

# Testy Skarbnik

Zawsze stosuj główny `AGENTS.md`. Zachowaj .NET 7 oraz EF Core 7.0.20; nie aktualizuj ich bez uzasadnienia i zgody.

Głównym celem tego skilla jest dodawanie lub ocena testów. Zmiana kodu aplikacji jest dopuszczalna wyłącznie wtedy, gdy minimalna zmiana jest konieczna do testowania. Implementację nowej funkcjonalności prowadź przez skill `funkcja`, a naprawę istniejącego błędu przez skill `naprawa`.

1. Sprawdź strukturę repozytorium, projekt `Skarbnik.Tests`, istniejące testy i kod objęty zadaniem.
2. Opieraj testy na istniejących klasach i metodach. Wskaż, jakie zachowanie lub przypadek brzegowy test zabezpieczy.
3. Przed edycją przedstaw plan i listę plików.
4. Umieszczaj testy w `Skarbnik.Tests`; testy EF Core uruchamiaj na oddzielnej bazie SQLite w pamięci.
5. Nie pobieraj connection stringów z `appsettings*.json` ani ustawień wdrożeniowych. Nie łącz się z SQL Serverem, nie uruchamiaj migracji ani nie zmieniaj produkcyjnej konfiguracji.
6. Nie usuwaj testów ani nie osłabiaj istniejących asercji.
7. Po zmianach uruchom `scripts/verify.ps1`. Analizuj błędy i poprawiaj tylko własne zmiany, najwyżej przez 3 pełne próby weryfikacji łącznie. Po trzecim niepowodzeniu przerwij i opisz problem.

Jeśli test wymaga zmiany kodu aplikacji, uwzględnij ją w planie i ogranicz do minimum. Raportuj zmienione pliki, testy, wynik skryptu, liczbę prób, ostrzeżenia i problemy. Nie wykonuj commitów ani push bez wyraźnej zgody użytkownika.
