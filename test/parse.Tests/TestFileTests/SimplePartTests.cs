using parse.Models;

namespace parse.Tests.TestFileTests
{
    public class SimplePartTests : ParserTests
    {
        private const string _fileName = TestFileNames.SimplePart;
        private readonly ConfigFile _result;

        public SimplePartTests()
        {
            _result = GetParsedConfigFile(_fileName);
        }
    }
}