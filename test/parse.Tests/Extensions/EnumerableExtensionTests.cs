using parse.Extensions;
using Xunit;

namespace parse.Tests.Extensions
{
    public class EnumerableExtensionTests
    {
        [Theory]
        [InlineData("part", NodeType.Part)]
        [InlineData("Part", NodeType.Part)]
        [InlineData("PART", NodeType.Part)]
        [InlineData("urlconfig", NodeType.UrlConfig)]
        [InlineData("urlConfig", NodeType.UrlConfig)]
        [InlineData("UrlConfig", NodeType.UrlConfig)]
        [InlineData("URLCONFIG", NodeType.UrlConfig)]
        public void NodeType_enums_are_not_strict(string input, NodeType expected)
        {
            var result = input.ToEnum<NodeType>();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("GarbageNodeType")]
        public void NodeType_returns_unknown_as_default(string input)
        {
            var result = input.ToEnumOrDefault<NodeType>();
            Assert.Equal(NodeType.Unknown, result);
        }
    }
}