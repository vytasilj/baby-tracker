namespace BabyTracker.Data;

public class MedicalExamEntry : ChildScopedEntity
{
    public string ExamType { get; set; } = string.Empty; // built-in key (e.g. "HipUltrasound") or free text for custom
    public bool IsBuiltIn { get; set; }
    public string? Result { get; set; }
    public string? Notes { get; set; }
}