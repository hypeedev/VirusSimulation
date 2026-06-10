# Virus Simulation

Konsolowa symulacja epidemii napisana w C#. Symuluje rozprzestrzenianie się wirusa na proceduralnie generowanej mapie z różnymi typami ludzi, zachowaniami ruchowymi i strategiami zapobiegawczymi.

## Wymagania

- .NET SDK 10.0 lub nowszy

## Budowanie

```sh
dotnet build
```

## Uruchamianie

```sh
dotnet run
```

Program pyta o:
- Typ wirusa (Flu / Covid / Rabies)
- Liczebność populacji (Workers, Students, Elders, Doctors, Hospitals)

## Uruchamianie testów

```sh
dotnet test tests/VirusSimulation.Tests
```

Testy używają xUnit i obejmują:
- Infekcję, leczenie, śmierć i odporność ludzi
- Przechodniość planszy, zapytania o jednostki i obliczanie zasięgu
- Właściwości wirusów i logikę rozprzestrzeniania
- Eksport statystyk do CSV
- Leczenie przez szpital i doktora

Wszystkie testy zależne od RNG używają deterministycznego ziarna (`GameRandom.Create(42)`),
co zapewnia powtarzalność. Interfejs `IRandom` pozwala wstrzykiwać RNG z ziarnem
w testach, podczas gdy w produkcji używane jest `GameRandom.Create()` (bez ziarna).

## Struktura projektu

```
Interfaces/       – ITile, IVirus, ISpreadLogic, IHealingAbility, IRandom
Models/
  Entities/       – Entity (bazowa), Hospital
  Humans/         – Human, Worker, Student, Elder, Doctor
  Map/            – Tile (enum)
  SpreadLogic/    – Uniform, DistanceWeighted, Focused spread
  Viruses/        – Flu, Covid, Rabies
Utilities/        – Board, MapGenerator, Renderer, Statistics, GameRandom
tests/            – projekt testowy xUnit
```

## Szczegóły symulacji

- Ludzie poruszają się losowo z opcjonalną migracją dalekodystansową między regionami
- Wirusy rozprzestrzeniają się z konfigurowalną zakaźnością, śmiertelnością i zasięgiem
- Blokada (lockdown) aktywuje się, gdy infekcja przekracza 80% żyjącej populacji
- Świadomość (awareness) zmniejsza ruchliwość w miarę wzrostu liczby zarażonych
- Szpitale i lekarze leczą pobliskich zarażonych przez 5 ticków
- Wyniki eksportowane do pliku `simulation.csv`
