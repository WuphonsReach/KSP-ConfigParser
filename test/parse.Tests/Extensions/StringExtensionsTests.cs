using Xunit;
using parse.Extensions;

namespace parse.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", null)]
        [InlineData("//", "", "")]
        [InlineData("A//", "A", "")]
        [InlineData("A//B", "A", "B")]
        [InlineData("A B C D  // BXY", "A B C D  ", " BXY")]
        [InlineData("B C D  // BXY//XYZ", "B C D  ", " BXY//XYZ")]
        public void ParseCorrectly(string input, string expectedData, string expectedComment)
        {
            var result = input.ParseLine();
            Assert.Equal(expectedData, result.Data);
            Assert.Equal(expectedComment, result.Comment);
        }
    }
}