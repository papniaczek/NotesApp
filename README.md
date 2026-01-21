# Aplikacja do notatek i zadań

Nasz projekt to aplikacja desktopowa służąca do zarządzania czasem i wiedzą. Pozwala na tworzenie listy zadań (To-Do) z priorytetami i terminami oraz prowadzenie notatek tekstowych w jednym, spójnym interfejsie.

Aplikacja została zbudowana w technologii **.NET 8** z wykorzystaniem frameworka **Avalonia UI**, zgodnie ze wzorcem architektonicznym **MVVM**.

---

##  Główne Funkcjonalności

### 1. Zarządzanie Zadaniami (Tasks)
- **Tworzenie:** Dodawanie zadań z priorytetem (High/Normal/Low), datą do wykonania i tagami.
- **Organizacja:** Automatyczne grupowanie zadań chronologicznie (Dzisiaj, Jutro, Nadchodzące, Bez terminu).
- **Status:** Możliwość oznaczania zadań jako wykonane (checkbox).

### 2. Notatki (Notes)
- Prosty edytor notatek z tytułem i treścią.
- System tagowania ułatwiający kategoryzację.
- Widok szczegółowy notatki.

### 3. Panel Statystyk (Dashboard)
- **Real-time:** Statystyki odświeżają się natychmiast po każdej zmianie (np. odhaczeniu zadania).
- **Metryki:**
  - Liczba zadań (wykonane / do zrobienia).
  - Zadania z krótkim deadlinem (termin w ciągu najbliższych 7 dni).
  - Rozkład priorytetów.

### 4. Filtrowanie i Wyszukiwanie
- Błyskawiczne wyszukiwanie po treści i tagach.
- Filtrowanie listy zadań według priorytetów.

---

##  Technologie i Architektura

Projekt został zrealizowany z naciskiem na czystość kodu i zastosowanie inżynieryjnych wzorców projektowych.

- **Język:** C# (.NET 8.0)
- **Framework UI:** Avalonia UI (Cross-platform)
- **Architektura:** MVVM (Model-View-ViewModel)

### Zastosowane Wzorce Projektowe

W aplikacji zaimplementowano 5 kluczowych wzorców:

1.  **Singleton (`AppManager`)**
    - Zapewnia jedno, centralne źródło danych dla całej aplikacji. Gwarantuje spójność listy notatek pomiędzy różnymi widokami.

2.  **Observer (`IObserver`, `TaskStatsObserver`...)**
    - Odpowiada za automatyczne odświeżanie panelu statystyk. Gdy dane w modelu się zmieniają, obserwatorzy przeliczają statystyki i aktualizują widok bez ingerencji użytkownika.

3.  **Command (`ICommand`, `AddEntryCommand`...)**
    - Oddziela logikę biznesową od interfejsu użytkownika. Przyciski w widoku nie zawierają kodu logicznego, lecz uruchamiają komendy.

4.  **Composite (`IEntryComponent`)**
    - Umożliwia traktowanie Zadań (`Task`) i Notatek (`Note`) jako elementów tego samego typu. Dzięki temu mogą znajdować się na jednej liście i być wspólnie filtrowane.

5.  **Builder (`TaskBuilder`)**
    - Upraszcza tworzenie skomplikowanych obiektów zadań, które posiadają wiele parametrów opcjonalnych (priorytet, data, kategoria).

---

## Instrukcja Instalacji i Uruchomienia

Aby uruchomić projekt na swoim komputerze, wykonaj poniższe kroki.

### Wymagania wstępne

- System operacyjny: Windows, macOS lub Linux.
- .NET 8.0 SDK - [Pobierz tutaj.](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Krok 1: Pobranie kodu

Otwórz terminal (lub konsolę Git Bash) i wpisz:
```
git clone https://github.com/papniaczek/NotesApp.git
cd NotesApp
```

### Krok 2: Uruchomienie z IDE (Zalecane)

Najwygodniejszym sposobem jest użycie edytora kodu:

1. Otwórz folder projektu w JetBrains Rider, Visual Studio 2022 lub Visual Studio Code.
2. Zaczekaj chwilę, aż edytor pobierze wymagane biblioteki (NuGet restore).
3. Naciśnij przycisk Run (lub klawisz F5).

### Krok 3: Uruchomienie z terminala

Możesz też uruchomić aplikację bezpośrednio z linii komend:
```
dotnet restore
dotnet run
```

Autorzy:
- Kacper Strześniewski
- Kacper Holcman
- Szymon Niemyjski
- Karolina Czaplicka
