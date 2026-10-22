---
name: review
description: Przegląda kod projektu pod kątem błędów, literówek, polskich nazw identyfikatorów i jakości kodu. Zapisuje raport w HTML bez modyfikowania kodu aplikacji.
---

# Code Review — Skarbnik

## Cel

Wykonaj dokładny przegląd kodu projektu i zapisz wyniki po polsku w czytelnym raporcie HTML.

## Zasady

1. Przeczytaj `AGENTS.md` i stosuj się do jego instrukcji.
2. Sprawdź aktualny stan Git przed rozpoczęciem pracy.
3. Nie cofaj, nie usuwaj ani nie nadpisuj istniejących zmian użytkownika.
4. Nie modyfikuj kodu aplikacji, konfiguracji ani testów. Twoim zadaniem jest wyłącznie analiza i utworzenie raportu HTML.
5. Nie zgłaszaj problemów bez uzasadnienia. Odróżniaj rzeczywiste błędy od sugestii stylistycznych.
6. Nie pokazuj haseł, tokenów, connection stringów ani innych sekretów w raporcie. Jeśli je znajdziesz, wskaż lokalizację bez ujawniania wartości.

## Co sprawdzić

### 1. Literówki i nazewnictwo

Sprawdź:
- literówki w nazwach klas, metod, właściwości, zmiennych i parametrów;
- błędy w komentarzach, komunikatach i tekstach interfejsu;
- niespójne nazewnictwo tych samych pojęć;
- nazwy nieczytelne, mylące lub niezgodne z konwencjami C#.

### 2. Polskie nazwy w kodzie

Wskaż identyfikatory zapisane po polsku, w szczególności:
- nazwy metod;
- nazwy klas i interfejsów;
- nazwy zmiennych i właściwości;
- nazwy parametrów i typów pomocniczych.

Dla każdej znalezionej nazwy zaproponuj angielski odpowiednik, np.:

- `PobierzUczniow()` → `GetStudentsAsync()` — jeśli metoda rzeczywiście działa asynchronicznie;
- `DodajWydatek()` → `AddExpense()`;
- `CzyZaplacono` → `IsPaid`.

Nie zakładaj, że każda polska nazwa jest błędem. Uwzględnij kontekst, istniejące konwencje i kompatybilność. Nie zalecaj zmiany nazw generowanych przez framework, istniejących migracji ani elementów, których zmiana mogłaby niepotrzebnie naruszyć zgodność.

**Nie zgłaszaj polskich tekstów interfejsu jako problemu.** Teksty takie jak „Dodaj wydatek”, „Nie zapłacono” czy „Szczegóły” mogą pozostać po polsku.

### 3. Błędy i ryzyka

Sprawdź między innymi:
- możliwe błędy logiczne i przypadki brzegowe;
- obsługę wartości null;
- obsługę wyjątków i błędów;
- walidację danych wejściowych;
- bezpieczeństwo i autoryzację;
- zapytania EF Core i potencjalne problemy wydajnościowe;
- duplikację kodu i niepotrzebną złożoność;
- zgodność ze strukturą projektu ASP.NET Core Razor Pages.

Nie traktuj przypuszczeń jako potwierdzonych błędów. Jeśli czegoś nie da się zweryfikować, zaznacz to w raporcie.

### 4. Testy

Sprawdź istniejące testy i oceń:
- czy obejmują ważne scenariusze;
- czy brakuje testów przypadków brzegowych;
- czy zmiany w kodzie mogłyby wymagać dodatkowych testów.

Nie modyfikuj testów. Możesz wskazać konkretne testy, które warto dodać.

### 5. Zakres analizy

- Przejrzyj zmiany w Git oraz istotne pliki powiązane z analizowanym kodem.
- Jeśli użytkownik nie wskazał konkretnego pliku, przeanalizuj projekt możliwie szeroko, z pominięciem katalogów generowanych, `bin`, `obj` i zależności zewnętrznych.
- Nie uruchamiaj migracji, nie zmieniaj bazy danych i nie wykonuj operacji wdrożeniowych.
- Jeżeli uruchamiasz testy lub inne polecenia, odnotuj ich rzeczywisty wynik.

## Raport HTML

Zapisz kompletny raport pod ścieżką:

`reports/code-review.html`

Jeśli katalog `reports` nie istnieje, utwórz go.

Raport ma być samodzielnym plikiem HTML z osadzonym CSS, bez zewnętrznych bibliotek i zależności internetowych. Powinien być czytelny po otwarciu w przeglądarce, także na mniejszym ekranie.

Raport powinien zawierać:

1. Tytuł „Raport przeglądu kodu — Skarbnik”.
2. Datę i godzinę analizy.
3. Podsumowanie: liczba znalezionych problemów według kategorii i poziomu ważności.
4. Osobną sekcję literówek.
5. Osobną sekcję polskich nazw identyfikatorów wraz z proponowanymi angielskimi odpowiednikami.
6. Sekcję błędów, ryzyk i sugestii dotyczących jakości kodu.
7. Sekcję brakujących lub zalecanych testów.
8. Wyniki uruchomionych kontroli i testów.
9. Listę przeanalizowanych obszarów oraz ograniczeń analizy.

Każde zgłoszenie powinno zawierać:
- poziom ważności: Krytyczny, Wysoki, Średni lub Niski;
- kategorię;
- plik i numer linii, jeśli można go wiarygodnie ustalić;
- aktualny zapis lub opis problemu;
- proponowaną poprawkę;
- krótkie uzasadnienie.

Rozróżnij potwierdzone błędy od rekomendacji. Nie zawyżaj poziomu ważności.

Zastosuj czytelne nagłówki, tabele lub karty, wyróżnienie poziomów ważności oraz podsumowanie na początku raportu.

Przy każdym uruchomieniu utwórz aktualny raport pod wskazaną ścieżką, zastępując poprzedni raport. Nie nadpisuj żadnych innych plików.

## Zakończenie

Po utworzeniu raportu:
- podaj pełną ścieżkę do pliku HTML;
- podsumuj najważniejsze ustalenia;
- poinformuj, czy uruchomiono testy i jaki był ich wynik;
- zaznacz, że kod aplikacji nie został zmodyfikowany.
