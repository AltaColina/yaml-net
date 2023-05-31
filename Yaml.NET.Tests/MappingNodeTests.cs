namespace Yaml.NET.Tests;

public sealed class MappingNodeTests
{
    [Fact]
    public void MappingOfScalars()
    {
        var mapping = new MappingNode
        {
            ["int"] = new ScalarNode(20),
            ["float"] = new ScalarNode(20.20),
            ["string"] = new ScalarNode("20"),
        };

        var yaml = YamlSerializer.Serialize(mapping);

        var expected = """
            int: 20
            float: 20.2
            string: "20"

            """;

        Assert.Equal(expected, yaml);
    }

    [Fact]
    public void MappingOfSequences()
    {
        var mapping = new MappingNode
        {
            ["seq1"] = new SequenceNode
            {
                new ScalarNode(20),
                new ScalarNode(20.20),
                new ScalarNode("20"),
            },
            ["seq2"] = new SequenceNode
            {
                new ScalarNode(20),
                new ScalarNode(20.20),
                new ScalarNode("20"),
            },
        };

        var yaml = YamlSerializer.Serialize(mapping);

        var expected = """
            seq1: 
              - 20
              - 20.2
              - "20"
            seq2: 
              - 20
              - 20.2
              - "20"

            """;

        Assert.Equal(expected, yaml);
    }

    [Fact]
    public void MappingOfMappings()
    {
        var mapping = new MappingNode
        {
            ["map1"] = new MappingNode
            {
                ["int"] = new ScalarNode(20),
                ["float"] = new ScalarNode(20.20),
                ["string"] = new ScalarNode("20"),
            },
            ["map2"] = new MappingNode
            {
                ["int"] = new ScalarNode(20),
                ["float"] = new ScalarNode(20.20),
                ["string"] = new ScalarNode("20"),
            },
        };

        var yaml = YamlSerializer.Serialize(mapping);

        var expected = """
            map1: 
              int: 20
              float: 20.2
              string: "20"
            map2: 
              int: 20
              float: 20.2
              string: "20"
            
            """;

        Assert.Equal(expected, yaml);
    }

    [Fact]
    public void MappingOfMixedNodes()
    {
        var mapping = new MappingNode
        {
            ["scalar1"] = new ScalarNode(20),
            ["seq1"] = new SequenceNode
            {
                new ScalarNode(20),
                new ScalarNode(20.20),
                new ScalarNode("20"),
            },
            ["map1"] = new MappingNode
            {
                ["int"] = new ScalarNode(20),
                ["float"] = new ScalarNode(20.20),
                ["string"] = new ScalarNode("20"),
            },
            ["seq2"] = new SequenceNode
            {
                new ScalarNode(20),
                new ScalarNode(20.20),
                new ScalarNode("20"),
            },
            ["scalar2"] = new ScalarNode(),
            ["map2"] = new MappingNode
            {
                ["int"] = new ScalarNode(20),
                ["float"] = new ScalarNode(20.20),
                ["string"] = new ScalarNode("20"),
            },
        };

        var yaml = YamlSerializer.Serialize(mapping);

        var expected = """
            scalar1: 20
            seq1: 
              - 20
              - 20.2
              - "20"
            map1: 
              int: 20
              float: 20.2
              string: "20"
            seq2: 
              - 20
              - 20.2
              - "20"
            scalar2: null
            map2: 
              int: 20
              float: 20.2
              string: "20"
            
            """;

        Assert.Equal(expected, yaml);
    }
}