# 🩺 Doctor Appointment System

[![.NET](https://img.shields.io/badge/.NET-Core-blue.svg)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-MVC-success.svg)]()
[![Database](https://img.shields.io/badge/Database-Entity_Framework-lightgrey.svg)]()

Kompleksowa aplikacja webowa do zarządzania i rezerwacji wizyt lekarskich. Projekt został zrealizowany w oparciu o klasyczny wzorzec **Model-View-Controller (MVC)**, co zapewnia czytelny podział obowiązków (Separation of Concerns) pomiędzy warstwą danych, logiką biznesową a interfejsem użytkownika.

## 🚀 Główne funkcjonalności (Features)

* **Autoryzacja i Uwierzytelnianie:** Pełny system rejestracji i logowania użytkowników (pacjentów i lekarzy).
* **Zarządzanie wizytami (CRUD):** * Przeglądanie dostępnych lekarzy i ich harmonogramów.
  * Rezerwacja wolnych terminów wizyt (Booking).
  * Przeglądanie własnych wizyt w dedykowanym panelu pacjenta.
  * Możliwość edycji i anulowania zaplanowanych wizyt.
* **Walidacja danych:** Bezpieczna walidacja formularzy zarówno po stronie klienta (JavaScript/jQuery Validation), jak i po stronie serwera.
* **Responsywny interfejs UI:** Widoki oparte na systemie gridowym (Bootstrap), dostosowane do urządzeń mobilnych i desktopowych.

## 🛠️ Stack Technologiczny

Projekt udowadnia umiejętność pracy na każdym etapie tworzenia aplikacji (Full-Stack), ze szczególnym naciskiem na **Backend i relacyjne bazy danych**.

**Backend:**
* C# / ASP.NET Core
* Architektura MVC (Controllers, Models, Views)
* LINQ (do zaawansowanego odpytywania kolekcji i bazy danych)

**Baza Danych:**
* Relacyjna baza danych (SQL)
* Entity Framework Core (Code-First Approach, system migracji)

**Frontend:**
* HTML5 / CSS3
* Razor Pages (`.cshtml` - dynamiczne renderowanie widoków z serwera)
* Bootstrap & jQuery

## 📂 Architektura Projektu (MVC)

Struktura katalogów ściśle odzwierciedla wzorzec projektowy MVC:

* `Controllers/` - Kontrolery (np. `AppointmentController`, `AccountController`) obsługujące żądania HTTP, komunikujące się z bazą danych i zwracające odpowiednie widoki.
* `Models/` - Modele domenowe (np. `Doctor`, `User`, `AppointmentSlot`) mapowane bezpośrednio na tabele w relacyjnej bazie danych.
* `Views/` - Warstwa prezentacji, odbierająca dane z kontrolerów i renderująca gotowy kod HTML dla przeglądarki.
* `Data/` - Konfiguracja kontekstu bazy danych (`ApplicationDbContext`).

## ⚙️ Uruchomienie lokalne (Setup)

Aby uruchomić projekt na swoim lokalnym środowisku, wykonaj poniższe kroki:

1. Sklonuj repozytorium:
   ```bash
   git clone [https://github.com/TwojLogin/doctor-appointment-system.git](https://github.com/TwojLogin/doctor-appointment-system.git)
