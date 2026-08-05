using BabyTracker.Data;

namespace BabyTracker.Tests;

public class PluralRulesTests
{
    [Theory]
    [InlineData(1, "One")]
    [InlineData(2, "Few")]
    [InlineData(3, "Few")]
    [InlineData(4, "Few")]
    [InlineData(0, "Other")]
    [InlineData(5, "Other")]
    [InlineData(11, "Other")]
    public void GetCategory_Czech_FollowsCzechGrammar(int count, string expected)
    {
        Assert.Equal(expected, PluralRules.GetCategory("cs", count));
    }

    [Theory]
    [InlineData(1, "One")]
    [InlineData(0, "Other")]
    [InlineData(2, "Other")]
    [InlineData(11, "Other")]
    public void GetCategory_English_UsesSimpleOneOtherSplit(int count, string expected)
    {
        Assert.Equal(expected, PluralRules.GetCategory("en", count));
    }

    [Fact]
    public void GetCategory_UnknownLanguage_FallsBackToDefaultRule()
    {
        // Languages without a specific rule (e.g. German, not yet added) should behave
        // like English's simple one/other split, not throw or misbehave.
        Assert.Equal("One", PluralRules.GetCategory("de", 1));
        Assert.Equal("Other", PluralRules.GetCategory("de", 5));
    }
}