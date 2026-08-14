using BabyTracker.Data;
using BabyTracker.App.Localization;

namespace BabyTracker.App.Services;

public class DailyTrackerSummaryService(
    EntryRepository<FeedingEntry> feedingRepository,
    SleepRepository sleepRepository,
    EntryRepository<DiaperEntry> diaperRepository,
    EntryRepository<TemperatureEntry> temperatureRepository,
    EntryRepository<WeightEntry> weightRepository,
    EntryRepository<PumpingEntry> pumpingRepository,
    SupplementRepository supplementRepository,
    MomSleepRepository momSleepRepository,
    UnitPreferenceService unitPreference)
{
    // Event-based trackers (logged multiple times a day) return today's count.
    // Value-based trackers (Temperature/Weight) return that specific day's reading, if any.
    public async Task<string> ComputeSummaryAsync(TrackerKind kind, Guid childId, DateOnly day)
    {
        var loc = LocalizationResourceManager.Instance;

        switch (kind)
        {
            case TrackerKind.Feeding:
                {
                    var entries = await feedingRepository.GetAllAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == day)}×";
                }
            case TrackerKind.Diaper:
                {
                    var entries = await diaperRepository.GetAllAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == day)}×";
                }
            case TrackerKind.Pumping:
                {
                    var entries = await pumpingRepository.GetAllAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == day)}×";
                }
            case TrackerKind.Supplement:
                {
                    var entries = await supplementRepository.GetEntriesAsync(childId);
                    return $"{entries.Count(e => DateOnly.FromDateTime(e.OccurredAt) == day)}×";
                }
            case TrackerKind.Sleep:
                {
                    var entries = await sleepRepository.GetAllAsync(childId);
                    var hours = SleepHoursCalculator.TotalHoursForDay(day, entries.Select(e => (e.StartTime, e.EndTime)), DateTime.Now);
                    return SleepFormatter.FormatTotalHours(hours);
                }
            case TrackerKind.MomSleep:
                {
                    var entries = await momSleepRepository.GetAllAsync();
                    var hours = SleepHoursCalculator.TotalHoursForDay(day, entries.Select(e => (e.StartTime, e.EndTime)), DateTime.Now);
                    return SleepFormatter.FormatTotalHours(hours);
                }
            case TrackerKind.Temperature:
                {
                    var entries = await temperatureRepository.GetAllAsync(childId);
                    var match = entries.FirstOrDefault(e => DateOnly.FromDateTime(e.OccurredAt) == day);
                    return match is null ? "—" : TemperatureFormatter.FormatForDisplay(match.ValueCelsius, unitPreference.Current, loc.NumberFormatCulture);
                }
            case TrackerKind.Weight:
                {
                    var entries = await weightRepository.GetAllAsync(childId);
                    var match = entries.FirstOrDefault(e => DateOnly.FromDateTime(e.OccurredAt) == day);
                    return match is null ? "—" : WeightFormatter.FormatForDisplay(match.WeightKg, unitPreference.Current, loc.NumberFormatCulture);
                }
            default:
                return "—";
        }
    }
}