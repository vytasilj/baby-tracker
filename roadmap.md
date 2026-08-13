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
- [ ] Per-tracker enable/disable in Settings (TrackerSetting entity already exists, not wired to any UI yet)

## Phase 3: Calendar & vaccinations
- [ ] Calendar with important events (doctor appointments, etc.)
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
- [ ] Pixel Pro temperature sensor via Health Connect: user measures with Google's native "Thermometer" app, which writes the reading into Health Connect; our app could then read it with the user's consent via
      androidx.health.connect.client. Confirmed the native Android library exists and is open source. Not yet confirmed: whether a maintained .NET/MAUI binding package exists,
      or whether we'd need to write a custom Android Binding Library ourselves — meaningfully more work than a typical NuGet integration. Investigate properly before committing.

---
*Update this file whenever a phase item is completed — check it off in the same commit as the feature, so this file always reflects reality, not memory.*