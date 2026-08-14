namespace BabyTracker.Data;

public class VaccineDefinition : SyncableEntity
{
    public string? BuiltInKey { get; set; }
    public string? Name { get; set; }
}