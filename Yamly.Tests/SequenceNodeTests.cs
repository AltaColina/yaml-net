namespace Yamly.Tests;

public sealed class SequenceNodeTests
{
    [Fact]
    public void SequenceOfScalars()
    {
        var sequence = new SequenceNode
        {
            new ScalarNode(20),
            new ScalarNode(20.20),
            new ScalarNode("20"),
        };

        var yaml = YamlSerializer.Serialize(sequence);

        var expected = """
            - 20
            - 20.2
            - "20"

            """;

        Assert.Equal(expected, yaml);
    }

    [Fact]
    public void SequenceOfSequences()
    {
        var sequence = new SequenceNode
        {
            new SequenceNode
            {
                new ScalarNode(20)
            },
            new SequenceNode
            {
                new ScalarNode(20.20),
                new ScalarNode("20")
            }
        };

        var yaml = YamlSerializer.Serialize(sequence);

        var expected = """
            -
              - 20
            -
              - 20.2
              - "20"

            """;

        Assert.Equal(expected, yaml);
    }

    [Fact]
    public void SequenceOfMappings()
    {
        var sequence = new SequenceNode
        {
            new MappingNode
            {
                ["integer"] = new ScalarNode(20)
            },
            new MappingNode
            {
                ["float"] = new ScalarNode(20.20),
                ["string"] = new ScalarNode("20")
            }
        };

        var yaml = YamlSerializer.Serialize(sequence);

        var expected = """
            -
              integer: 20
            -
              float: 20.2
              string: "20"

            """;

        Assert.Equal(expected, yaml);
    }

    [Fact]
    public void SequenceOfMixedNodes()
    {
        var sequence = new SequenceNode
        {
            new ScalarNode(true),
            new SequenceNode
            {
                new ScalarNode(20)
            },
            new MappingNode
            {
                ["float"] = new ScalarNode(20.20),
                ["string"] = new ScalarNode("20")
            },
            new ScalarNode("20"),
            new MappingNode
            {
                ["float"] = new ScalarNode(20.20),
                ["string"] = new ScalarNode("20")
            },
            new SequenceNode
            {
                new ScalarNode(20)
            },
        };

        var yaml = YamlSerializer.Serialize(sequence);

        var expected = """
            - true
            -
              - 20
            -
              float: 20.2
              string: "20"
            - "20"
            -
              float: 20.2
              string: "20"
            -
              - 20

            """;

        Assert.Equal(expected, yaml);
    }
}