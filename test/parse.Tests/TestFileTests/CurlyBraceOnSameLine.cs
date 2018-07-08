using System.Linq;
using parse.Models;
using Xunit;

namespace parse.Tests.TestFileTests
{
    public class CurlyBraceOnSameLine : ParserTests
    {
        private const string _fileName = TestFileNames.CurlyBraceOnSameLine;
        private readonly ConfigFile _configFile;

        public CurlyBraceOnSameLine()
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
        public void Part_node_has_resource_node()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var resourceNode = firstNode.Nodes.First(x => x.Type == NodeType.Resource);
            Assert.NotNull(resourceNode);
        }

        [Fact]
        public void Part_node_has_module_node()
        {
            var firstNode = _configFile.RootNode.Nodes.First();
            var resourceNode = firstNode.Nodes.First(x => x.Type == NodeType.Module);
            Assert.NotNull(resourceNode);
        }
    }
}