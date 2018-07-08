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
            var result = _configFile.Nodes.First();
            Assert.Equal(NodeType.Part, result.Type);
        }
    }
}