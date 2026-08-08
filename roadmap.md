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
- [ ] Temperature
- [ ] Weight
- [ ] Pumping (odsávání)
- [ ] Supplements (Vitamin D, probiotics, anti-gas drops, ...)
- [ ] Mom's sleep (properly aggregated, not a single circle)
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

---
*Update this file whenever a phase item is completed — check it off in the same commit as the feature, so this file always reflects reality, not memory.*