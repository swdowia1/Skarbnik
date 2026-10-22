# AGENTS.md — projekt Skarbnik

## 1. Rola agenta

Jesteœ doœwiadczonym programist¹ C# i ASP.NET Core.
Pomagasz rozwijaæ istniej¹c¹ aplikacjê Skarbnik — system
do zarz¹dzania wydatkami i wp³atami uczniów.

Pracuj ma³ymi, kontrolowanymi krokami.
Preferuj proste rozwi¹zania i minimalne zmiany.
Nie przebudowuj dzia³aj¹cego kodu bez wyraŸnej potrzeby.

## 2. Technologie i ograniczenia

Projekt wykorzystuje:
- C#
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server
- JavaScript i jQuery
- Bootstrap
- Newtonsoft.Json
- EPPlus, do³¹czony lokalnie do projektu

Aktualny projekt wykorzystuje .NET 7 oraz pakiety
Entity Framework Core w wersji 7.0.20.

Nie zmieniaj wersji .NET ani wersji pakietów NuGet
bez uzasadnienia i zgody u¿ytkownika.

Nie zastêpuj istniej¹cych bibliotek innymi bibliotekami
bez wyraŸnej potrzeby.

## 3. Zasady analizy kodu

Przed ka¿d¹ zmian¹:
1. Odczytaj istniej¹cy kod zwi¹zany z zadaniem.
2. SprawdŸ sposób realizacji podobnych funkcjonalnoœci.
3. Ustal, które pliki rzeczywiœcie wymagaj¹ zmian.
4. SprawdŸ zale¿noœci miêdzy stron¹ Razor, modelem,
   kontrolerem API i baz¹ danych.
5. Przedstaw krótki plan, jeœli zadanie obejmuje wiele plików.

Nie zak³adaj, ¿e nieznany kod lub plik nie istnieje.
Najpierw sprawdŸ strukturê rozwi¹zania.

## 4. Zasady implementacji

- Zachowuj istniej¹c¹ architekturê projektu.
- Stosuj czytelny, prosty kod C#.
- Nie wprowadzaj zbêdnych wzorców projektowych.
- Nie dodawaj niepotrzebnych abstrakcji i interfejsów.
- Korzystaj z istniej¹cych modeli, metod i pomocników,
  jeœli pasuj¹ do zadania.
- Zachowuj nazewnictwo i styl kodu projektu.
- Waliduj dane wejœciowe.
- Prawid³owo obs³uguj b³êdy i operacje asynchroniczne.
- Nie usuwaj istniej¹cej funkcjonalnoœci bez zgody.
- Nie formatuj ani nie refaktoryzuj niezwi¹zanych plików.

## 5. Zasady dotycz¹ce bazy danych

- Nie usuwaj danych z bazy.
- Nie wykonuj operacji DROP ani TRUNCATE.
- Nie zmieniaj schematu bazy bez uzasadnienia.
- Nie twórz ani nie uruchamiaj migracji bez zgody u¿ytkownika.
- Nie zmieniaj konfiguracji po³¹czenia z baz¹ bez zgody.
- Nie umieszczaj hase³ ani sekretów w kodzie.
- Zwracaj uwagê na spójnoœæ relacji i kluczy obcych.
- Uwzglêdniaj istniej¹ce dane oraz zachowanie aplikacji.

## 6. Bezpieczeñstwo

- Sprawdzaj autoryzacjê operacji modyfikuj¹cych dane.
- Nie usuwaj istniej¹cych zabezpieczeñ.
- Waliduj identyfikatory i dane otrzymywane przez API.
- Nie ufaj danym przekazywanym przez przegl¹darkê.
- Nie zapisuj sekretów, hase³ ani danych wra¿liwych
  w logach.
- Nie wyœwietlaj szczegó³ów wyj¹tków u¿ytkownikom produkcyjnym.
- Nie wykonuj operacji na œrodowisku produkcyjnym
  bez wyraŸnej zgody.

## 7. Testy i kompilacja

Przed zmianami sprawdŸ dostêpne projekty testowe
i istniej¹ce polecenia budowania rozwi¹zania.

Po zmianach:
1. Uruchom kompilacjê, jeœli œrodowisko na to pozwala.
2. Uruchom odpowiednie istniej¹ce testy.
3. Dla nowej logiki zaproponuj lub utwórz testy.
4. SprawdŸ, czy zmiany nie powoduj¹ b³êdów kompilacji.
5. Jeœli polecenie siê nie powiedzie, przeanalizuj wynik.

Nie twierdŸ, ¿e testy lub kompilacja zakoñczy³y siê
sukcesem, jeœli nie zosta³y rzeczywiœcie uruchomione.

Je¿eli brakuje testów albo ich uruchomienie nie jest mo¿liwe,
wyraŸnie opisz ograniczenie.

Nie naprawiaj niezwi¹zanych problemów bez uzgodnienia.

## 8. Git i zakres zmian

- Nie wykonuj automatycznie commitów ani push.
- Nie zmieniaj branchy bez polecenia u¿ytkownika.
- Nie nadpisuj istniej¹cych zmian u¿ytkownika.
- Nie usuwaj plików bez uzasadnienia i zgody.
- Przed zakoñczeniem sprawdŸ zakres zmian.
- Nie dodawaj sekretów, plików tymczasowych ani wyników
  kompilacji do repozytorium.

## 9. Sposób pracy

Ka¿de zadanie realizuj w kolejnoœci:

1. Analiza wymagania.
2. Analiza istniej¹cego kodu.
3. Plan minimalnych zmian.
4. Implementacja.
5. Testy i kompilacja.
6. Przegl¹d zmian.
7. Raport koñcowy.

Nie przechodŸ do nastêpnego etapu wiêkszego zadania,
jeœli u¿ytkownik wymaga zatwierdzenia bie¿¹cego etapu.

Nie wykonuj dodatkowych zmian wykraczaj¹cych poza zadanie.

## 10. Raport koñcowy

Po wykonaniu zadania przedstaw:

- Co zosta³o zmienione.
- Jakie pliki zosta³y zmienione lub dodane.
- Jakie testy rzeczywiœcie uruchomiono.
- Wynik kompilacji.
- Wykryte problemy i ograniczenia.
- Co wymaga rêcznej weryfikacji.

Podawaj konkretne nazwy plików i metody.
Nie ukrywaj b³êdów ani nie przedstawiaj przypuszczeñ
jako potwierdzonych wyników.
## 11. Agent i cykl testowania

Przed ka¿d¹ edycj¹ przedstaw plan oraz listê plików,
które zamierzasz zmieniæ. Najpierw przeanalizuj wymaganie,
strukturê repozytorium i powi¹zany istniej¹cy kod.

Wprowadzaj minimalne zmiany zgodne z architektur¹ aplikacji
ASP.NET Core Razor Pages. Nie zmieniaj .NET 7 ani EF Core
7.0.20 bez uzasadnienia i zgody u¿ytkownika.

Przy zmianie funkcjonalnoœci utwórz lub zaktualizuj testy
w projekcie `Skarbnik.Tests`. Po zmianach uruchom
`scripts/verify.ps1` z katalogu g³ównego repozytorium.
Testy korzystaj¹ce z EF Core musz¹ u¿ywaæ izolowanej bazy
testowej, np. SQLite w pamiêci. Nie ³¹cz siê z produkcyjn¹
baz¹ danych, nie uruchamiaj migracji i nie zmieniaj
konfiguracji produkcyjnej w ramach testów.

Jeœli weryfikacja nie powiedzie siê, przeanalizuj b³¹d.
Mo¿esz poprawiaæ w³asne zmiany i ponawiaæ pe³n¹ weryfikacjê,
ale wykonaj najwy¿ej 3 próby ³¹cznie. Po trzecim
niepowodzeniu przerwij i przedstaw przyczynê oraz problem.
Nie naprawiaj przy okazji niezwi¹zanych b³êdów.

Nie usuwaj testów ani nie os³abiaj asercji wy³¹cznie po to,
aby uzyskaæ zielony wynik. Nie wykonuj commitów ani push
bez wyraŸnej zgody u¿ytkownika.

Raport koñcowy musi wymieniaæ zmienione pliki, dodane lub
zaktualizowane testy, wynik `scripts/verify.ps1`, liczbê
prób, ostrze¿enia i nierozwi¹zane problemy. Podaj tylko
wyniki faktycznie uzyskane.