# Aplikacja bazodanowa
 
 ## Wstęp
 Projekt zawiera integrację z zewnętrznym API, służącym do pobierania danych z czujników jakości powietrza, oraz obsługę lokalnej bazy danych SQLite.
 
 ## Opis funkcjonalności
 Aplikacja pozwala na 
 - pobieranie danych z API ```PurpleAir```
 - zapisywanie danych do lokalnej bazy danych SQLite
 - odczyt zapisanych pomiarów z bazy

 ## Struktura projektu
 Projekt zawiera następujące pliki:
 - **Program.cs** - Główna logika aplikacji konsolowej, obsługuje wybór opcji, komunikację z API i bazą danych.
 - **Sensor.cs** - Model encji reprezentującej czujnik (```sensor```) z przypisanymi pomiarami.
 - **Measurement.cs** - Model encji reprezentującej pojedynczy pomiar z czujnika.
 - **AppDbContext.cs** - Konfiguracja bazy danych SQLite i definicja relacji między encjami.
 - **PurpleAirClient.cs** - Klient HTTP odpowiedzialny za pobieranie danych z PurpleAir API.
 - **PurpleAirResponse.cs** - Modele służące do deserializacji odpowiedzi JSON z API PurpleAir.

 ## Wykorzystane technologie
 - .NET 8.0
 - C#
