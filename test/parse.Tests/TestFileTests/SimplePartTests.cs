using System.Linq;
using parse.Models;
using Xunit;

namespace parse.Tests.TestFileTests
{
    public class SimplePartTests : ParserTests
    {
        private const string _fileName = TestFileNames.SimplePart;
        private readonly ConfigFile _configFile;

        public SimplePartTests()
        {
            _configFile = GetParsedConfigFile(_fileName);
        }

        [Fact]
        public void First_top_level_node_should_be_part()
        {
            var result = _configFile.RootNode.Nodes.First();
            Assert.Equal(NodeType.Part, result.Type);
        }

        [Theory]
        [InlineData("name", "HighGainAntenna")]
        [InlineData("module", "Part")]
        [InlineData("rescaleFactor", "1")]
        [InlineData("maxTemp", "2000")]
        [InlineData("bulkheadProfiles", "srf")]
        public void Part_node_has_attribute_values(string name, string value)
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var attribute = firstNode.AttributeDefinitions.First(x => x.Name == name);
            Assert.Equal(value, attribute.Value);
        }

        [Fact]
        public void Part_node_has_model_node()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var node = firstNode.Nodes.First(x => x.Type == NodeType.Model);
            Assert.NotNull(node);
        }

        [Theory]
        [InlineData("model", "Squad/Parts/Misc/ABC/HighGainAntenna")]
        public void Part_model_node_has_attribute_values(string name, string value)
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var node = firstNode.Nodes.First(x => x.Type == NodeType.Model);
            var attribute = node.AttributeDefinitions.First(x => x.Name == name);
            Assert.Equal(value, attribute.Value);
        }

    }
}