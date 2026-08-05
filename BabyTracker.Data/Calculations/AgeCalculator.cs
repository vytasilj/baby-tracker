namespace BabyTracker.Data;

public enum AgeUnit { NotBornYet, Days, Weeks, Months }

public record AgeDescription(AgeUnit Unit, int Primary, int Secondary);

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

    public static AgeDescription Calculate(DateOnly birthDate, DateOnly asOf)
    {
        var totalDays = TotalDays(birthDate, asOf);
        if (totalDays < 0) return new AgeDescription(AgeUnit.NotBornYet, 0, 0);
        if (totalDays < 14) return new AgeDescription(AgeUnit.Days, totalDays, 0);

        if (totalDays < 60)
        {
            return new AgeDescription(AgeUnit.Weeks, totalDays / 7, totalDays % 7);
        }

        var (months, remDays) = MonthsAndDays(birthDate, asOf);
        return new AgeDescription(AgeUnit.Months, months, remDays);
    }
}