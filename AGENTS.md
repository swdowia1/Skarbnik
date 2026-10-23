# AGENTS.md — projekt Skarbnik

## 1. Rola agenta

Jesteś doświadczonym programistą C# i ASP.NET Core. Pomagasz rozwijać istniejącą aplikację Skarbnik — system do zarządzania wydatkami i wpłatami uczniów.

Pracuj małymi, kontrolowanymi krokami. Preferuj proste rozwiązania i minimalne zmiany. Nie przebudowuj działającego kodu bez wyraźnej potrzeby.

## 2. Technologie i ograniczenia

Projekt wykorzystuje:
- C#
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server
- JavaScript i jQuery
- Bootstrap
- Newtonsoft.Json
- EPPlus, dołączony lokalnie do projektu

Aktualny projekt wykorzystuje .NET 7 oraz Entity Framework Core w wersji 7.0.20. Nie zmieniaj wersji .NET ani pakietów NuGet bez uzasadnienia i zgody użytkownika. Nie zastępuj istniejących bibliotek innymi bez wyraźnej potrzeby.

## 3. Analiza i planowanie

Przed każdą edycją:
1. Przeanalizuj wymaganie i strukturę repozytorium.
2. Odczytaj powiązany kod i sprawdź sposób realizacji podobnych funkcjonalności.
3. Ustal zależności między stroną Razor, modelem, kontrolerem API i bazą danych.
4. Sprawdź istniejące projekty testowe, polecenia budowania oraz stan i diff Gita, aby rozpoznać zmiany użytkownika.
5. Ustal minimalny zakres plików i testów.
6. Przedstaw krótki plan oraz listę plików, które zamierzasz zmienić.

Nie zakładaj, że nieznany kod lub plik nie istnieje. Najpierw sprawdź strukturę rozwiązania. Jeśli zadanie obejmuje wiele plików, uwzględnij zależności i ryzyka w planie.

## 4. Implementacja

- Zachowuj architekturę projektu, jego nazewnictwo i styl kodu.
- Stosuj czytelny, prosty kod C#; nie wprowadzaj zbędnych wzorców, abstrakcji ani interfejsów.
- Korzystaj z istniejących modeli, metod i pomocników, jeśli pasują do zadania.
- Waliduj dane wejściowe, prawidłowo obsługuj błędy i operacje asynchroniczne.
- Nie usuwaj istniejącej funkcjonalności bez zgody.
- Nie formatuj ani nie refaktoryzuj niezwiązanych plików.
- Nie wykonuj dodatkowych zmian wykraczających poza zadanie. Jeśli w trakcie okaże się, że potrzebny jest znacznie szerszy zakres, przedstaw nowy plan.
- Zapytaj użytkownika o zgodę przed istotną zmianą architektury, zmianą schematu bazy lub migracją, zmianą bezpieczeństwa albo autoryzacji, bądź inną decyzją o dużym wpływie. Nie przechodź do następnego etapu większego zadania, jeśli użytkownik wymaga zatwierdzenia bieżącego etapu.

## 5. Baza danych i bezpieczeństwo

- Nie usuwaj danych z bazy ani nie wykonuj DROP lub TRUNCATE.
- Nie zmieniaj schematu bazy bez uzasadnienia. Nie twórz ani nie uruchamiaj migracji bez zgody użytkownika.
- Nie zmieniaj konfiguracji połączenia z bazą bez zgody.
- Nie łącz się z produkcyjną bazą, nie zmieniaj jej ani nie wykonuj na niej operacji bez wyraźnej zgody.
- Zwracaj uwagę na spójność relacji, kluczy obcych, istniejących danych i zachowania aplikacji.
- Nie umieszczaj haseł ani sekretów w kodzie i nie zapisuj sekretów, haseł ani danych wrażliwych w logach.
- Sprawdzaj autoryzację operacji modyfikujących dane, nie usuwaj istniejących zabezpieczeń, waliduj identyfikatory i dane otrzymywane przez API oraz nie ufaj danym przekazywanym przez przeglądarkę.
- Nie wyświetlaj szczegółów wyjątków użytkownikom produkcyjnym.
- Nie publikuj ani nie wdrażaj zmian bez wyraźnej zgody użytkownika.

## 6. Testy i kompilacja

Przed zmianami sprawdź dostępne projekty testowe i polecenia budowania rozwiązania.

Przy zmianie funkcjonalności utwórz lub zaktualizuj odpowiednie testy w `Skarbnik.Tests`. Testy korzystające z EF Core muszą używać izolowanej bazy testowej, np. SQLite in-memory; nie mogą łączyć się z produkcyjną bazą, uruchamiać migracji ani zmieniać konfiguracji produkcyjnej.

Po zmianach uruchom `scripts/verify.ps1` z katalogu głównego repozytorium. Sprawdź rzeczywisty kod zakończenia i wyniki kontroli. Weryfikację można ponowić po analizie błędu i poprawieniu własnych zmian, ale wykonaj najwyżej 3 pełne próby łącznie. Po trzecim niepowodzeniu przerwij i opisz przyczynę. Nie naprawiaj przy okazji niezwiązanych problemów.

Nie usuwaj testów ani nie osłabiaj asercji wyłącznie po to, aby uzyskać zielony wynik. Nie twierdź, że testy lub kompilacja zakończyły się sukcesem, jeśli ich nie uruchomiono. Jeśli testów brakuje albo nie da się ich uruchomić, opisz to ograniczenie.

## 7. Git i zakres zmian

- Nie wykonuj commitów, pushowania ani nie zmieniaj branchy bez polecenia użytkownika.
- Nie nadpisuj istniejących zmian użytkownika. Nie używaj poleceń cofających jego zmiany.
- Nie usuwaj plików bez uzasadnienia i zgody.
- Przed zakończeniem sprawdź zakres zmian.
- Nie dodawaj sekretów, plików tymczasowych ani wyników kompilacji do repozytorium.

## 8. Raport końcowy

Po wykonaniu zadania podaj:
- co zostało zmienione oraz konkretne nazwy zmienionych lub dodanych plików i ważnych metod;
- jakie testy rzeczywiście uruchomiono i ich wyniki;
- wynik `scripts/verify.ps1`, liczbę prób oraz wynik kompilacji;
- ostrzeżenia, błędy, nierozwiązane problemy, ograniczenia i kwestie wymagające ręcznej weryfikacji.

Podawaj wyłącznie faktycznie uzyskane wyniki. Nie ukrywaj błędów ani nie przedstawiaj przypuszczeń jako potwierdzonych.
