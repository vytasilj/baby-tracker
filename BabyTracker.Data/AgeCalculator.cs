namespace BabyTracker.Data;

public static class AgeCalculator
{
    public static int TotalDays(DateOnly birthDate, DateOnly asOf) => asOf.DayNumber - birthDate.DayNumber;

    public static (int Months, int Days) MonthsAndDays(DateOnly birthDate, DateOnly asOf)
    {
        var months = (asOf.Year - birthDate.Year) * 12 + (asOf.Month - birthDate.Month);
        var candidate = birthDate.AddMonths(months);
        if (candidate > asOf)
        {
            months--;
            candidate = birthDate.AddMonths(months);
        }
        return (months, asOf.DayNumber - candidate.DayNumber);
    }

    // Chooses the most readable unit depending on age: days for newborns,
    // weeks for the first couple of months, then months — matches how parents
    // naturally talk about a baby's age at different stages.
    public static string Describe(DateOnly birthDate, DateOnly asOf)
    {
        var totalDays = TotalDays(birthDate, asOf);
        if (totalDays < 0) return "Not born yet";
        if (totalDays < 14) return totalDays == 1 ? "1 day" : $"{totalDays} days";

        if (totalDays < 60)
        {
            var weeks = totalDays / 7;
            var days = totalDays % 7;
            return days == 0 ? $"{weeks} weeks" : $"{weeks}w {days}d";
        }

        var (months, remDays) = MonthsAndDays(birthDate, asOf);
        return remDays == 0 ? $"{months} months" : $"{months}m {remDays}d";
    }
}