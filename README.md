# Virus Simulation

A console-based epidemic simulation in C#. Simulates virus spread across a procedurally generated map with different human types, movement behaviors, and mitigation strategies.

## Requirements

- .NET SDK 10.0 or later

## Build

```sh
dotnet build
```

## Run

```sh
dotnet run
```

The program prompts for:
- Virus type (Flu / Covid / Rabies)
- Population counts (Workers, Students, Elders, Doctors, Hospitals)

## Run Tests

```sh
dotnet test tests/VirusSimulation.Tests
```

Tests use xUnit and cover:
- Human infection, healing, death, and immunity
- Board walkability, entity queries, and range calculations
- Virus properties and spread logic
- Statistics CSV export
- Hospital and Doctor healing behavior

All RNG-dependent tests use a deterministic seed (`GameRandom.Create(42)`),
making them reproducible across runs. The `IRandom` interface allows injecting
seeded RNGs for testing while using `GameRandom.Create()` (unseeded) in production.

## Project Structure

```
Interfaces/       – ITile, IVirus, ISpreadLogic, IHealingAbility, IRandom
Models/
  Entities/       – Entity base, Hospital
  Humans/         – Human, Worker, Student, Elder, Doctor
  Map/            – Tile enum
  SpreadLogic/    – Uniform, DistanceWeighted, Focused spread
  Viruses/        – Flu, Covid, Rabies
Utilities/        – Board, MapGenerator, Renderer, Statistics, GameRandom
tests/            – xUnit test project
```

## Simulation Details

- Humans move randomly with optional long-distance migration between regions
- Viruses spread with configurable infectivity, mortality, and range
- Lockdown activates when infection exceeds 80% of alive population
- Awareness reduces movement as infection grows
- Hospitals and Doctors heal nearby infected humans over 5 ticks
- Results exported to `simulation.csv`
