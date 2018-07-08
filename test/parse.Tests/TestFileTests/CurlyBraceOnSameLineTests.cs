using System.Linq;
using parse.Models;
using Xunit;

namespace parse.Tests.TestFileTests
{
    public class CurlyBraceOnSameLineTests : ParserTests
    {
        private const string _fileName = TestFileNames.CurlyBraceOnSameLine;
        private readonly ConfigFile _configFile;

        public CurlyBraceOnSameLineTests()
        {
            _configFile = GetParsedConfigFile(_fileName);
        }

        [Fact]
        public void First_top_level_node_must_be_part()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            Assert.Equal(NodeType.Part, firstNode.Type);
        }

        [Fact]
        public void Part_has_correct_InputLines_count()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            Assert.Equal(5, firstNode.InputLines.Count);
        }

        [Theory]
        [InlineData("name", "Example")]
        public void Part_has_attribute_and_value(string attributeName, string expectedValue)
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var nameAttribute = firstNode.Attributes.First(x => x.Name == attributeName);
            Assert.Equal(expectedValue, nameAttribute.Value);
        }

        [Fact]
        public void Part_node_has_resource_node()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var resourceNode = firstNode.Nodes.First(x => x.Type == NodeType.Resource);
            Assert.NotNull(resourceNode);
        }

        [Fact]
        public void Part_resource_has_correct_InputLines_count()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var resourceNode = firstNode.Nodes.First(x => x.Type == NodeType.Resource);
            Assert.Equal(5, resourceNode.InputLines.Count);
        }

        [Theory]
        [InlineData("name", "LiquidFuel")]
        [InlineData("amount", "400")]
        public void Part_resource_has_attribute_and_value(string attributeName, string expectedValue)
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var resourceNode = firstNode.Nodes.First(x => x.Type == NodeType.Resource);
            var nameAttribute = resourceNode.Attributes.First(x => x.Name == attributeName);
            Assert.Equal(expectedValue, nameAttribute.Value);
        }

        [Fact]
        public void Part_node_has_module_node()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var moduleNode = firstNode.Nodes.First(x => x.Type == NodeType.Module);
            Assert.NotNull(moduleNode);
        }

        [Fact]
        public void Part_module_has_correct_InputLines_count()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var moduleNode = firstNode.Nodes.First(x => x.Type == NodeType.Module);
            Assert.Equal(4, moduleNode.InputLines.Count);
        }
    }
}