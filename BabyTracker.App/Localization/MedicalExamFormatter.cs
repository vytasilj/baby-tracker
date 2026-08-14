using BabyTracker.Data;

namespace BabyTracker.App.Localization;

public static class MedicalExamFormatter
{
    private static readonly string[] BuiltInKeys = ["HipUltrasound", "HearingScreening", "MetabolicScreening"];

    public static IReadOnlyList<string> BuiltInExamKeys => BuiltInKeys;

    public static string DisplayName(MedicalExamEntry entry) =>
        entry.IsBuiltIn ? LocalizationResourceManager.Instance[$"MedicalExam_{entry.ExamType}"] : entry.ExamType;
}