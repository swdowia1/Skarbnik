---
name: funkcja
description: "Implementuje nowe funkcjonalności w projekcie SkarbnikCodex. Analizuje kod, przedstawia plan, wprowadza zmiany, dodaje testy i uruchamia weryfikację."
---
# Skill: Realizacja funkcjonalno�ci � SkarbnikCodex

## Cel

Zrealizuj ��dan� funkcjonalno�� w istniej�cym projekcie SkarbnikCodex w spos�b kontrolowany, prosty i zgodny z architektur� aplikacji.

Pracuj od analizy przez plan do implementacji i weryfikacji.

## 1. Zasady nadrz�dne

1. Przeczytaj `AGENTS.md` i stosuj zawarte w nim zasady.
2. Wykorzystuj istniej�cy kod, konwencje, serwisy, modele i testy.
3. Preferuj najmniejsz� zmian�, kt�ra poprawnie realizuje wymaganie.
4. Nie przebudowuj architektury, je�li zadanie tego nie wymaga.
5. Nie dodawaj nowych pakiet�w ani bibliotek bez uzasadnionej potrzeby.
6. Nie zmieniaj niezwi�zanych plik�w.
7. Nie usuwaj ani nie cofaj istniej�cych zmian u�ytkownika.
8. Nie wykonuj commit�w, pushowania ani wdro�e� bez wyra�nej zgody u�ytkownika.
9. Nie zak�adaj, �e testy przesz�y � uruchom je i sprawd� rzeczywisty wynik.
10. Komunikuj si� z u�ytkownikiem po polsku.

## 2. Etap analizy

Przed implementacj�:

1. Ustal, co dok�adnie ma robi� funkcjonalno��.
2. Sprawd� struktur� projektu i powi�zane pliki.
3. Przeanalizuj odpowiednie modele, serwisy, PageModel, strony Razor, kontrolery API i istniej�ce testy.
4. Sprawd� `git status` i `git diff`, aby pozna� bie��ce zmiany u�ytkownika.
5. Ustal, jakie pliki trzeba zmieni� i jakie testy b�d� potrzebne.
6. Zidentyfikuj ryzyka, zale�no�ci i mo�liwe skutki uboczne.

Nie zak�adaj, �e plik jest niezmieniony tylko dlatego, �e nie zosta� zmodyfikowany przez bie��ce zadanie.

## 3. Plan przed implementacj�

Przed rozpocz�ciem zmian przedstaw kr�tki plan zawieraj�cy:

- cel i spos�b realizacji;
- list� plik�w do zmiany lub utworzenia;
- plan test�w;
- potencjalne ryzyka.

W przypadku prostego, jednoznacznego zadania przedstaw plan i przejd� do realizacji.

**Zapytaj u�ytkownika o zgod� przed implementacj�, je�eli zadanie wymaga istotnej zmiany architektury, wielu niezale�nych zmian, zmiany schematu bazy danych, migracji, zmiany bezpiecze�stwa lub autoryzacji albo innej decyzji o du�ym wp�ywie na projekt.**

Je�li wymagania s� niejednoznaczne i wyb�r wp�ynie na zachowanie aplikacji, zadaj pytanie zamiast zgadywa�.

Nie zatrzymuj si� na planowaniu, je�li zadanie jest jasne i nie wymaga dodatkowej zgody.

## 4. Implementacja

Po zaakceptowaniu planu, je�li by�a wymagana zgoda:

1. Wprowad� zaplanowane zmiany.
2. Zachowaj istniej�cy styl kodu i nazewnictwo projektu.
3. Stosuj istniej�ce rozwi�zania zamiast tworzy� r�wnoleg�e mechanizmy.
4. Korzystaj z asynchroniczno�ci tam, gdzie jest uzasadniona.
5. Dodaj walidacj� i obs�ug� b��d�w odpowiedni� do danej funkcjonalno�ci.
6. Uwzgl�dnij autoryzacj� i bezpiecze�stwo, je�li zmiana dotyczy operacji chronionych.
7. Nie umieszczaj sekret�w ani danych dost�powych w kodzie.
8. Aktualizuj istniej�ce testy lub dodaj nowe, gdy zmiana tego wymaga.
9. Nie zmieniaj zachowania niezwi�zanego z zadaniem.
10. Je�li podczas pracy oka�e si�, �e potrzebny jest znacznie szerszy zakres zmian, zatrzymaj si� i przedstaw nowy plan.

## 5. Testy i weryfikacja

Po implementacji:

1. Sprawd� diff, aby upewni� si�, �e zmiany odpowiadaj� zadaniu.
2. Uruchom istniej�cy skrypt `scripts/verify.ps1`, zgodnie z jego aktualn� zawarto�ci� i instrukcjami projektu.
3. Sprawd�, czy aplikacja si� buduje.
4. Uruchom odpowiednie testy automatyczne, w tym testy z projektu `Skarbnik.Tests`, je�li s� dost�pne i odpowiednie dla zmiany.
5. Sprawd�, czy testy nie wymagaj� produkcyjnej bazy danych lub innych niedost�pnych us�ug.
6. Nie modyfikuj produkcyjnej bazy danych i nie wykonuj migracji bez zgody.
7. Je�li weryfikacja zako�czy si� b��dem, przeanalizuj przyczyn� i popraw b��dy wynikaj�ce z w�asnych zmian.
8. Nie ukrywaj niepowodze� i nie zmieniaj test�w wy��cznie po to, aby uzyska� zielony wynik.
9. Je�li test�w nie da si� uruchomi�, wyja�nij dlaczego.
10. Nie naprawiaj niezwi�zanych, istniej�cych ostrze�e� i b��d�w bez potrzeby.

Nie uznawaj samego uruchomienia skryptu za dow�d sukcesu. Sprawd� jego kod zako�czenia oraz wyniki poszczeg�lnych kontroli.

## 6. Ochrona zmian u�ytkownika

- Nigdy nie u�ywaj `git reset --hard`.
- Nie wykonuj `git checkout --`, `git restore` ani innych operacji cofaj�cych zmiany u�ytkownika.
- Nie nadpisuj plik�w u�ytkownika bez sprawdzenia ich aktualnej zawarto�ci.
- Nie usuwaj nieznanych plik�w ani nieporzuconych zmian.
- Nie wykonuj `git commit`, `git push`, merge ani wdro�enia bez zgody.
- Nie dodawaj automatycznie wszystkich plik�w do commita.
- Przed zako�czeniem sprawd� `git status` i `git diff`.

## 7. Zako�czenie zadania

Po zako�czeniu przedstaw raport po polsku zawieraj�cy:

### Wykonane zmiany
- co zosta�o dodane lub zmienione;
- list� zmodyfikowanych plik�w;
- kr�tkie wyja�nienie najwa�niejszych decyzji.

### Weryfikacja
- wynik `scripts/verify.ps1`;
- wynik kompilacji;
- liczb� zaliczonych i niezaliczonych test�w, je�li mo�na j� ustali�;
- ostrze�enia lub b��dy, kt�re pozosta�y.

### Pozosta�e kwestie
- ograniczenia, ryzyka lub czynno�ci wymagaj�ce r�cznego sprawdzenia;
- informacj�, czy u�ytkownik musi wykona� dodatkowy krok.

Nie deklaruj sukcesu, je�li nie zosta� potwierdzony wynikami.

## Zasada ko�cowa

Twoim celem jest dostarczenie dzia�aj�cej funkcjonalno�ci przy mo�liwie najmniejszym zakresie zmian, z testami potwierdzaj�cymi poprawno�� i bez naruszania pracy u�ytkownika.