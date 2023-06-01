namespace Yamly.Tests;

public sealed class ScalarNodeTests
{
    [Fact]
    public void Null()
    {
        var scalar = new ScalarNode();
        var yaml = YamlSerializer.Serialize(scalar);
        var expected = $"null{Environment.NewLine}";
        Assert.Equal(expected, yaml);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void Boolean(string expected, bool value)
    {
        var scalar = new ScalarNode(value);
        var yaml = YamlSerializer.Serialize(scalar);
        expected = $"{expected}{Environment.NewLine}";
        Assert.Equal(expected, yaml);
    }

    [Theory]
    [InlineData("-1", -1)]
    [InlineData("0", 0)]
    [InlineData("1", 1)]
    public void Integer(string expected, long value)
    {
        var scalar = new ScalarNode(value);
        var yaml = YamlSerializer.Serialize(scalar);
        expected = $"{expected}{Environment.NewLine}";
        Assert.Equal(expected, yaml);
    }

    [Theory]
    [InlineData("-1.7976931348623157E+308", -1.7976931348623157E+308)]
    [InlineData("0.1", 0.1)]
    [InlineData("1.7976931348623157E+308", 1.7976931348623157E+308)]
    public void Float(string expected, double value)
    {
        var scalar = new ScalarNode(value);
        var yaml = YamlSerializer.Serialize(scalar);
        expected = $"{expected}{Environment.NewLine}";
        Assert.Equal(expected, yaml);
    }

    [Theory]
    [InlineData("\"string\"", "string")]
    [InlineData("", "")]
    [InlineData("null", null)]
    public void String(string expected, string value)
    {
        var scalar = new ScalarNode(value);
        var yaml = YamlSerializer.Serialize(scalar);
        expected = $"{expected}{Environment.NewLine}";
        Assert.Equal(expected, yaml);
    }
}