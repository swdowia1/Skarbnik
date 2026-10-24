---
name: code-review
description: Przegląda zmiany w kodzie Skarbnik pod kątem błędów, regresji, bezpieczeństwa i brakujących testów. Używaj do przeglądu diffu i powiązanego kodu bez jego modyfikowania.
---

# Code review — SkarbnikCodex

## Cel

Oceń zmiany w repozytorium pod kątem zgodności z wymaganiem, istniejącą architekturą i zachowaniem aplikacji. Skup się na konkretnych błędach, regresjach, ryzykach bezpieczeństwa oraz brakujących testach. Analizuj C#, ASP.NET Core Razor Pages, API, EF Core, JavaScript i testy.

Ten skill dotyczy przede wszystkim przeglądu zmian w Git. Istniejący skill `review` służy do szerszego audytu projektu i raportu HTML; nie powielaj jego przeglądu literówek i nazw identyfikatorów, chyba że mają wpływ na poprawność zmiany.

## Zasady

- Przeczytaj `AGENTS.md` i przestrzegaj jego ograniczeń, w tym wersji .NET 7 i EF Core 7.0.20.
- Zacznij od `git status` oraz diffu zmian niestage’owanych i stage’owanych. Nie zakładaj, że wszystkie zmiany należą do bieżącego zadania; nie nadpisuj ani nie cofaj pracy użytkownika.
- Porównaj zmiany z wymaganiem zadania. Jeśli wymaganie jest niejasne lub niedostępne, zaznacz ograniczenie zamiast zakładać intencję.
- Najpierw przeanalizuj zmienione fragmenty, a następnie odczytaj powiązane strony Razor, PageModel, kontrolery/API, serwisy, modele, konfigurację i testy, gdy są potrzebne do potwierdzenia przepływu lub skutków zmiany.
- Nie edytuj ani nie naprawiaj kodu. Nie wykonuj commitów, push, migracji ani operacji na bazie danych. Nie ujawniaj sekretów ani wartości connection stringów; w razie ich wykrycia wskaż bezpiecznie samą lokalizację.
- Nie przedstawiaj przypuszczeń jako potwierdzonych błędów. Oddziel potwierdzone problemy od pytań, ryzyk i rekomendacji. Zgłaszaj tylko ustalenia poparte kodem lub wymaganiem.
- Oceń, czy istotne scenariusze zmiany mają testy. Wskaż brakujące przypadki konkretnie; nie zmieniaj testów.

## Obszary przeglądu

Zależnie od zakresu zmian sprawdź:

- poprawność logiki, wartości brzegowe, null, obsługę błędów i zachowanie asynchroniczne;
- walidację i autoryzację, zaufanie do danych klienta, obsługę sekretów i możliwość ujawnienia danych;
- zgodność Razor Pages, PageModel, API i serwisów oraz niezamierzone zmiany istniejącego zachowania;
- zapytania i zapisy EF Core, spójność relacji, śledzenie encji oraz wpływ na istniejące dane i schemat;
- JavaScript, selektory DOM, zdarzenia, obsługę błędów i zgodność z istniejącym interfejsem;
- testy dla nowych lub zmienionych ścieżek, w tym przypadki brzegowe i izolację bazy testowej.

Nie zakładaj, że test lub kompilacja przeszły, jeśli ich nie uruchomiono. Uruchamiaj je tylko wtedy, gdy użytkownik tego oczekuje lub jest to potrzebne do rozstrzygnięcia konkretnego ustalenia; stosuj wówczas zasady `AGENTS.md` i nie łącz się z produkcyjną bazą.

## Raport

Dla każdego potwierdzonego problemu podaj:

- **Ważność:** Krytyczna, Wysoka, Średnia lub Niska;
- **Lokalizację:** plik i numer linii albo mały, jednoznaczny zakres zmienionego kodu;
- **Uzasadnienie:** konkretny sposób, w jaki zmiana powoduje błąd, regresję, ryzyko lub narusza wymaganie;
- **Proponowane rozwiązanie:** minimalna poprawka, bez jej wdrażania.

Umieszczaj najważniejsze problemy na początku. Nie podnoś ważności sugestii stylistycznych. Jeśli nie ma potwierdzonych problemów, napisz to wprost oraz podaj przeanalizowany zakres i ograniczenia (np. pominięte pliki, brak kontekstu wymagania lub nieuruchomione kontrole). Wymień wykonane kontrole i ich rzeczywiste wyniki.
