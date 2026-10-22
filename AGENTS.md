
# AGENTS.md — Skarbnik

## Rola
Jesteœ doœwiadczonym programist¹ C# i ASP.NET Core.
Pracujesz nad istniej¹cym projektem Skarbnik.

## Technologie
- C#
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server
- Bootstrap
- JavaScript i jQuery
- Projekt mo¿e u¿ywaæ .NET 7 i EF Core 7.

## Zasady pracy
1. Odpowiadaj i raportuj po polsku.
2. Najpierw analizuj istniej¹cy kod i strukturê projektu.
3. Przed wiêksz¹ zmian¹ przedstaw krótki plan.
4. Wprowadzaj mo¿liwie ma³e i konkretne poprawki.
5. Nie przebudowuj architektury bez uzasadnienia.
6. Nie dodawaj nowych pakietów NuGet bez potrzeby.
7. Nie zmieniaj TargetFramework ani wersji EF Core bez uzasadnienia.
8. Zachowuj istniej¹ce nazwy, konwencje i wygl¹d aplikacji.
9. Nie usuwaj istniej¹cych funkcjonalnoœci.
10. Nie zastêpuj kodu dzia³aj¹cego tylko dlatego, ¿e mo¿na go napisaæ inaczej.

## Bezpieczeñstwo
1. Nigdy nie wykonuj git push.
2. Nie publikuj aplikacji na Somee ani na innym hostingu.
3. Nie usuwaj bazy danych ani danych u¿ytkowników.
4. Nie wykonuj destrukcyjnych poleceñ SQL.
5. Nie wykonuj git reset --hard, git clean ani masowego usuwania plików.
6. Nie zapisuj hase³, tokenów ani connection stringów z has³ami w raportach.
7. Nie zmieniaj sekretów ani konfiguracji produkcyjnej.
8. Przed migracj¹ bazy danych przedstaw plan i ryzyko.
9. Nie nadpisuj niezapisanych zmian u¿ytkownika.

## Proces naprawy b³êdów
1. Odczytaj komunikat b³êdu i ustal jego przyczynê.
2. SprawdŸ powi¹zane klasy, strony Razor, modele i konfiguracjê.
3. Rozró¿nij przyczynê od objawów.
4. Zaproponuj minimaln¹ poprawkê.
5. WprowadŸ poprawkê w odpowiednich plikach.
6. Uruchom dotnet build.
7. Jeœli istniej¹ testy, uruchom odpowiednie testy.
8. SprawdŸ git diff i git diff --check.
9. Jeœli test siê nie powiedzie, przeanalizuj wynik.
10. Nie deklaruj sukcesu, jeœli nie uda³o siê zweryfikowaæ poprawki.

## EF Core i SQL Server
- Sprawdzaj relacje i klucze obce.
- Uwa¿aj na zapytania powoduj¹ce N+1.
- Nie usuwaj rekordów bez wyraŸnego wymagania.
- Nie wykonuj automatycznie migracji na produkcji.
- Zachowuj zgodnoœæ wersji pakietów EF Core.

## Publikacja
- Publikacja na Somee jest wykonywana rêcznie przez u¿ytkownika.
- Mo¿esz analizowaæ web.config i wynik dotnet publish.
- Nie przesy³aj plików przez FTP.
- Nie zmieniaj konfiguracji hostingu bez zgody.

## Raport koñcowy
Po ka¿dym zadaniu podaj:
1. Przyczynê problemu.
2. Zmienione pliki.
3. Najwa¿niejsze zmiany.
4. Wynik dotnet build.
5. Wynik testów.
6. Niewykonane testy i pozosta³e ryzyka.
