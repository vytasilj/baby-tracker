# Baby Tracker – Roadmap

## Phase 1: Core foundation
- [x] Child profile setup (name, birth date)
- [x] Home screen with age display
- [x] Light/dark theme toggle
- [x] Localization infrastructure (resx, EN default, CS, system-language detection, live switching)
- [x] Settings screen
- [x] Data model for Feeding, Sleep, Diaper entries
- [x] Actually recording and viewing Feeding, Sleep, Diaper entries
- [x] Multi-child support in the UI (data model already has ChildId everywhere, but there's no way to add/switch between children yet)

## Phase 2: Extended trackers
- [x] Temperature
- [x] Weight
- [x] Unit system (Metric/Imperial) with centralized, testable formatters
- [x] Pumping (odsávání)
- [x] Supplements (Vitamin D, probiotics, anti-gas drops, ...)
- [x] Mom's sleep (properly aggregated, not a single circle)
- [x] Per-tracker enable/disable in Settings (TrackerSetting entity already exists, not wired to any UI yet)

## Phase 3: Calendar & vaccinations
- [x] Calendar with important events (doctor appointments, etc.)
- [ ] Vaccination history/tracking
- [ ] Hip ultrasound / newborn screening records

## Phase 4: Statistics
- [ ] Weight gain over week/month/custom range
- [ ] Sleep trend graphs (baby + mom)
- [ ] Daily summary view for any past day (reuse DailySummaryCalculator + Home's card UI, just with a date picker instead of always "today")

## Phase 5: Multi-parent sync
- [ ] Sync via a shared Google Drive file (each parent's own Google account, no custom backend)
- [ ] Last-write-wins merge per entry (using UpdatedAt), soft-delete tombstones
- [ ] Manual + automatic background sync

## Phase 6: Play Store release prep
- [ ] App icon, splash screen
- [ ] Privacy policy (required for Play Store listing)
- [ ] Signing configuration
- [ ] Store listing content

## Future investigations (not committed, just ideas to check later)
- [ ] Pixel Pro temperature sensor via Health Connect: user measures with Google's
      native "Thermometer" app, which writes the reading into Health Connect; our app
      could then read it with the user's consent. Found a viable, actively maintained
      .NET MAUI package: Shiny.Health (github.com/shinyorg/health, NuGet: Shiny.Health) —
      has explicit DataType.BodyTemperature support with a working GetBodyTemperature()
      API for both Android Health Connect and iOS HealthKit. (Compared against
      Kebechet/Maui.Health, which lists body temperature as platform-supported but not
      yet wrapped in its own library — Shiny.Health is the better fit.) Setup involves
      Android manifest permissions + an activity-alias for Android 14+, documented in
      the package README. Not yet tried hands-on — do a small isolated spike before
      committing to build this into the real app.
      Note for Play Store prep (Phase 6): apps reading health data must fill in the
      "Health data collection and use" and "Data retention policy" sections in Play
      Console's Data Safety form, regardless of which package we use.