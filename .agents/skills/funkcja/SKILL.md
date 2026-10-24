---
name: funkcja
description: "Implementuje nowe funkcjonalności w projekcie SkarbnikCodex. Analizuje kod, przedstawia plan, wprowadza zmiany, dodaje testy i uruchamia weryfikację."
---
# Skill: Realizacja funkcjonalności — SkarbnikCodex

## Cel

Zrealizuj żądaną funkcjonalność w istniejącym projekcie SkarbnikCodex w sposób kontrolowany, prosty i zgodny z architekturą aplikacji.

Pracuj od analizy przez plan do implementacji i weryfikacji.

## 1. Zasady nadrzędne

1. Przeczytaj `AGENTS.md` i stosuj zawarte w nim zasady.
2. Wykorzystuj istniejący kod, konwencje, serwisy, modele i testy.
3. Preferuj najmniejszą zmianę, która poprawnie realizuje wymaganie.
4. Nie przebudowuj architektury, jeśli zadanie tego nie wymaga.
5. Nie dodawaj nowych pakietów ani bibliotek bez uzasadnionej potrzeby.
6. Nie zmieniaj niezwiązanych plików.
7. Nie usuwaj ani nie cofaj istniejących zmian użytkownika.
8. Nie wykonuj commitów, pushowania ani wdrożeń bez wyraźnej zgody użytkownika.
9. Nie zakładaj, że testy przeszły — uruchom je i sprawdź rzeczywisty wynik.
10. Komunikuj się z użytkownikiem po polsku.

## 2. Etap analizy

Przed implementacją:

1. Ustal, co dokładnie ma robić funkcjonalność.
2. Sprawdź strukturę projektu i powiązane pliki.
3. Przeanalizuj odpowiednie modele, serwisy, PageModel, strony Razor, kontrolery API i istniejące testy.
4. Sprawdź `git status` i `git diff`, aby poznać bieżące zmiany użytkownika.
5. Ustal, jakie pliki trzeba zmienić i jakie testy będą potrzebne.
6. Zidentyfikuj ryzyka, zależności i możliwe skutki uboczne.

Nie zakładaj, że plik jest niezmieniony tylko dlatego, że nie został zmodyfikowany przez bieżące zadanie.

## 3. Plan przed implementacją

Przed rozpoczęciem zmian przedstaw krótki plan zawierający:

- cel i sposób realizacji;
- listę plików do zmiany lub utworzenia;
- plan testów;
- potencjalne ryzyka.

W przypadku prostego, jednoznacznego zadania przedstaw plan i przejdź do realizacji.

**Zapytaj użytkownika o zgodę przed implementacją, jeżeli zadanie wymaga istotnej zmiany architektury, wielu niezależnych zmian, zmiany schematu bazy danych, migracji, zmiany bezpieczeństwa lub autoryzacji albo innej decyzji o dużym wpływie na projekt.**

Jeśli wymagania są niejednoznaczne i wybór wpłynie na zachowanie aplikacji, zadaj pytanie zamiast zgadywać.

Nie zatrzymuj się na planowaniu, jeśli zadanie jest jasne i nie wymaga dodatkowej zgody.

## 4. Implementacja

Po zaakceptowaniu planu, jeśli była wymagana zgoda:

1. Wprowadź zaplanowane zmiany.
2. Zachowaj istniejący styl kodu i nazewnictwo projektu.
3. Stosuj istniejące rozwiązania zamiast tworzyć równoległe mechanizmy.
4. Korzystaj z asynchroniczności tam, gdzie jest uzasadniona.
5. Dodaj walidację i obsługę błędów odpowiednią do danej funkcjonalności.
6. Uwzględnij autoryzację i bezpieczeństwo, jeśli zmiana dotyczy operacji chronionych.
7. Nie umieszczaj sekretów ani danych dostępowych w kodzie.
8. Aktualizuj istniejące testy lub dodaj nowe, gdy zmiana tego wymaga.
9. Nie zmieniaj zachowania niezwiązanego z zadaniem.
10. Jeśli podczas pracy okaże się, że potrzebny jest znacznie szerszy zakres zmian, zatrzymaj się i przedstaw nowy plan.

## 5. Testy i weryfikacja

Po implementacji:

1. Sprawdź diff, aby upewnić się, że zmiany odpowiadają zadaniu.
2. Uruchom istniejący skrypt `scripts/verify.ps1`, zgodnie z jego aktualną zawartością i instrukcjami projektu.
3. Sprawdź, czy aplikacja się buduje.
4. Uruchom odpowiednie testy automatyczne, w tym testy z projektu `Skarbnik.Tests`, jeśli są dostępne i odpowiednie dla zmiany.
5. Sprawdź, czy testy nie wymagają produkcyjnej bazy danych lub innych niedostępnych usług.
6. Nie modyfikuj produkcyjnej bazy danych i nie wykonuj migracji bez zgody.
7. Jeśli weryfikacja zakończy się błędem, przeanalizuj przyczynę i popraw błędy wynikające z własnych zmian.
8. Nie ukrywaj niepowodzeń i nie zmieniaj testów wyłącznie po to, aby uzyskać zielony wynik.
9. Jeśli testów nie da się uruchomić, wyjaśnij dlaczego.
10. Nie naprawiaj niezwiązanych, istniejących ostrzeżeń i błędów bez potrzeby.

Nie uznawaj samego uruchomienia skryptu za dowód sukcesu. Sprawdź jego kod zakończenia oraz wyniki poszczególnych kontroli.

## 6. Ochrona zmian użytkownika

- Nigdy nie używaj `git reset --hard`.
- Nie wykonuj `git checkout --`, `git restore` ani innych operacji cofających zmiany użytkownika.
- Nie nadpisuj plików użytkownika bez sprawdzenia ich aktualnej zawartości.
- Nie usuwaj nieznanych plików ani nieporzuconych zmian.
- Nie wykonuj `git commit`, `git push`, merge ani wdrożenia bez zgody.
- Nie dodawaj automatycznie wszystkich plików do commita.
- Przed zakończeniem sprawdź `git status` i `git diff`.

## 7. Zakończenie zadania

Po zakończeniu przedstaw raport po polsku zawierający:

### Wykonane zmiany
- co zostało dodane lub zmienione;
- listę zmodyfikowanych plików;
- krótkie wyjaśnienie najważniejszych decyzji.

### Weryfikacja
- wynik `scripts/verify.ps1`;
- wynik kompilacji;
- liczbę zaliczonych i niezaliczonych testów, jeśli można ją ustalić;
- ostrzeżenia lub błędy, które pozostały.

### Pozostałe kwestie
- ograniczenia, ryzyka lub czynności wymagające ręcznego sprawdzenia;
- informację, czy użytkownik musi wykonać dodatkowy krok.

Nie deklaruj sukcesu, jeśli nie został potwierdzony wynikami.

## Zasada końcowa

Twoim celem jest dostarczenie działającej funkcjonalności przy możliwie najmniejszym zakresie zmian, z testami potwierdzającymi poprawność i bez naruszania pracy użytkownika.
