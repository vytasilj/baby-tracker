namespace BabyTracker.Data;

public class VaccinationEntry : ChildScopedEntity
{
    public Guid VaccineDefinitionId { get; set; }
    public VaccineDefinition? Vaccine { get; set; }

    // Null = "given today" (OccurredAt on ChildScopedEntity is the given date).
    // Set = this is a planned/reminder entry for a future dose, not yet administered.
    public DateOnly? DueDate { get; set; }

    public int? DoseNumber { get; set; }
    public string? Notes { get; set; }

    public bool IsGiven => DueDate is null;
}