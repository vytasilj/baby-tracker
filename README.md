# Baby Tracker

A privacy-first Android app for tracking a baby's feeding, sleep, diapers, and more — built to replace a paper postpartum diary that ran out of room after the first week. Everything stays on your device; no cloud backend, no third-party servers, no accounts beyond what you choose to connect yourself.

![CI](https://github.com/vytasilj/baby-tracker/actions/workflows/ci.yml/badge.svg)

## Features

- Eight trackers: feeding, sleep (clock-in/clock-out with an "in progress" indicator), diapers, temperature, weight, pumping, supplements (built-in list + custom entries with hide/restore), and mom's sleep (shared across the whole family, independent of any single child)
- Multi-child support: add, edit, switch between, and remove children, with the app remembering the last-active child between launches
- A customizable Home screen showing today's summary as tappable cards, with per-tracker visibility toggles in Settings
- Full localization (English default, Czech included) with a from-scratch resx-based system supporting proper language-specific pluralization (e.g. Czech's one/few/many word forms) and live language switching without restarting the app
- Metric/Imperial unit system (°C/°F, kg/lb) with all data stored canonically in metric — historical values stay accurate no matter what a user later chooses to display
- Light/dark theme, respecting the system default until the user picks explicitly
- Soft-delete throughout (tombstone-based), laying the groundwork for future multi-device sync

## Tech stack

- .NET 10 / .NET MAUI (Android)
- CommunityToolkit.Mvvm (MVVM with source-generated observable properties and commands)
- Entity Framework Core + SQLite, with a generic `EntryRepository<T>` shared across most trackers via a common `ChildScopedEntity` base class
- CommunityToolkit.Maui (toast notifications)
- xUnit (unit tests for date/plural/unit-conversion logic and repository behavior, using an in-memory SQLite connection)
- GitHub Actions CI (with test result reporting via `dorny/test-reporter`)

## Getting started

**Prerequisites:** .NET 10 SDK with the MAUI workload (`dotnet workload install maui`), Android SDK, a physical Android device or emulator

```bash
git clone https://github.com/vytasilj/baby-tracker.git
cd baby-tracker/BabyTracker.App
dotnet build -t:Run -f net10.0-android
```

Data (config, SQLite database) lives entirely on-device in the app's private storage — nothing is sent anywhere.

## Running tests

```bash
dotnet test BabyTracker.Tests/BabyTracker.Tests.csproj
```

## Roadmap

See [ROADMAP.md](roadmap.md) for what's built and what's planned (calendar, vaccinations, statistics, multi-parent sync via a shared Google Drive file, Play Store release).