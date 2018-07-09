using System.Linq;
using parse.Extensions;
using parse.Models;
using Xunit;

namespace parse.Tests.TestFileTests
{
    public class MMCacheSimpleExampleTests : ParserTests
    {
        private const string _fileName = TestFileNames.MMCacheSimpleExample;
        private readonly ConfigFile _configFile;

        public MMCacheSimpleExampleTests()
        {
            _configFile = GetParsedConfigFile(_fileName);
        }

        private const string activeTextureManagerConfig = "ACTIVE_TEXTURE_MANAGER_CONFIG";

        [Fact]
        public void Can_find_activeTextureManagerConfig_node()
        {
            var nodes = _configFile.RootNode.Descendants();

            var node = nodes.First(x => x.TypeIdentifier == activeTextureManagerConfig);
            Assert.Equal(2, node.InputLines.First().BlockDepth);
            Assert.Equal(2, node.AttributeDefinitions.Count);

            var overridesNode = node.Nodes.First(x => x.TypeIdentifier == "OVERRIDES");
            Assert.Equal(3, overridesNode.InputLines.First().BlockDepth);
            Assert.Equal(0, overridesNode.AttributeDefinitions.Count);

            var scanSatNode = overridesNode.Nodes.First(x => x.TypeIdentifier == "SCANsat/Icons/.*");
            Assert.Equal(4, scanSatNode.InputLines.First().BlockDepth);
            Assert.Equal(5, scanSatNode.AttributeDefinitions.Count);
        }
    }
}
