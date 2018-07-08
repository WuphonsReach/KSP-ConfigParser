using Xunit;
using parse.Extensions;

namespace parse.Tests.Extensions
{
    public class StringExtensionsTests
    {
        //TODO: Use MemberData and split this test into two separate tests

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", null)]
        [InlineData("//", "", "")]
        [InlineData("A//", "A", "")]
        [InlineData("A//B", "A", "B")]
        [InlineData("A B C D  // BXY", "A B C D  ", " BXY")]
        [InlineData("B C D  // BXY//XYZ", "B C D  ", " BXY//XYZ")]
        [InlineData("=", "=", null)]
        [InlineData("=//", "=", "")]
        [InlineData("x=3//C", "x=3", "C")]
        [InlineData("PART {", "PART {", null)]
        [InlineData("PART { v = 2 // test", "PART { v = 2 ", " test")]
        [InlineData("y=4 //C=DV", "y=4 ", "C=DV")]
        [InlineData("PART { // Y=4", "PART { ", " Y=4")]
        public void ParseLine_works_correctly(string input, string expectedData, string expectedComment)
        {
            var result = input.ParseLine();
            Assert.Equal(expectedData, result.Data);
            Assert.Equal(expectedComment, result.Comment);
        }
    }
}