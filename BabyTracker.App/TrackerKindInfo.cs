using BabyTracker.App.Localization;

namespace BabyTracker.App;

public static class TrackerKindInfo
{
    public static string Icon(TrackerKind kind) => kind switch
    {
        TrackerKind.Feeding => "🍼",
        TrackerKind.Sleep => "😴",
        TrackerKind.Diaper => "🧷",
        TrackerKind.Temperature => "🌡️",
        TrackerKind.Weight => "⚖️",
        TrackerKind.Pumping => "🤱",
        TrackerKind.Supplement => "💊",
        TrackerKind.MomSleep => "😌",
        TrackerKind.Calendar => "🗓️",
        TrackerKind.Vaccination => "💉",
        _ => "❔"
    };

    public static string Label(TrackerKind kind)
    {
        var loc = LocalizationResourceManager.Instance;
        return kind switch
        {
            TrackerKind.Feeding => loc["Feeding_Title"],
            TrackerKind.Sleep => loc["Sleep_Title"],
            TrackerKind.Diaper => loc["Diaper_Title"],
            TrackerKind.Temperature => loc["Temperature_Title"],
            TrackerKind.Weight => loc["Weight_Title"],
            TrackerKind.Pumping => loc["Pumping_Title"],
            TrackerKind.Supplement => loc["Supplement_Title"],
            TrackerKind.MomSleep => loc["MomSleep_Title"],
            TrackerKind.Calendar => loc["Calendar_Title"],
            TrackerKind.Vaccination => loc["Vaccination_Title"],
            _ => "?"
        };
    }
}