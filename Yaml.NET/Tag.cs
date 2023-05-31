namespace Yaml.NET;

public sealed record class Tag(string Name)
{
    public static Tag Null { get; } = new("!!null");
    public static Tag Boolean { get; } = new("!!bool");
    public static Tag Integer { get; } = new("!!int");
    public static Tag Float { get; } = new("!!float");
    public static Tag String { get; } = new("!!str");
    public static Tag Sequence { get; } = new("!!seq");
    public static Tag Mapping { get; } = new("!!map");
}
