namespace Yaml.NET;

public sealed record class ScalarNode(Tag Tag, ScalarKind ScalarKind, object? Value) : Node(NodeKind.Scalar, Tag)
{
    public ScalarNode() : this(Tag.Null, ScalarKind.Null, null) { }
    public ScalarNode(bool value) : this(Tag.Boolean, ScalarKind.Boolean, value) { }
    public ScalarNode(long value) : this(Tag.Integer, ScalarKind.Integer, value) { }
    public ScalarNode(double value) : this(Tag.Float, ScalarKind.Float, value) { }
    public ScalarNode(string value) : this(Tag.String, ScalarKind.String, value) { }
}
