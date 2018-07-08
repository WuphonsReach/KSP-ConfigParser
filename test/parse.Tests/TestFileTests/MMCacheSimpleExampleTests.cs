using parse.Models;

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
    }
}