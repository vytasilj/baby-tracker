namespace BabyTracker.Data;

public class SupplementDefinition : SyncableEntity
{
    // Set to a stable key (e.g. "VitaminD") for built-in supplements — the display
    // name is translated from resx at display time. Null + Name set for custom ones,
    // which the user typed themselves and can't be translated.
    public string? BuiltInKey { get; set; }
    public string? Name { get; set; }
}