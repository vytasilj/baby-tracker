using BabyTracker.Data;

namespace BabyTracker.App.Localization;

public static class FeedingFormatter
{
    public static string FormatTypeAndDetail(FeedingType type, BreastSide? side, int? amountMl)
    {
        var loc = LocalizationResourceManager.Instance;

        return type switch
        {
            FeedingType.Breast when side is { } s => $"{loc["Feeding_Type_Breast"]} ({FormatSide(s)})",
            FeedingType.Breast => loc["Feeding_Type_Breast"],
            FeedingType.Bottle when amountMl is { } ml => $"{loc["Feeding_Type_Bottle"]} · {ml} ml",
            FeedingType.Bottle => loc["Feeding_Type_Bottle"],
            FeedingType.Solid => loc["Feeding_Type_Solid"],
            _ => ""
        };
    }

    public static string FormatSide(BreastSide side)
    {
        var loc = LocalizationResourceManager.Instance;
        return side switch
        {
            BreastSide.Left => loc["Feeding_Side_Left"],
            BreastSide.Right => loc["Feeding_Side_Right"],
            BreastSide.Both => loc["Feeding_Side_Both"],
            _ => ""
        };
    }
}