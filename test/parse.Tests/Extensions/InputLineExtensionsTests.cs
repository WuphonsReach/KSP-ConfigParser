using System.Linq;
using parse.Extensions;
using parse.Models;
using Xunit;

namespace parse.Tests.Extensions
{
    public class InputLineExtensionsTests
    {
        [Theory]
        [InlineData("x", "x")]
        [InlineData("x{", "x", "{")]
        [InlineData("}x{", "}", "x", "{")]
        [InlineData("}{}{", "}", "{", "}", "{")]
        [InlineData("}}{{", "}", "}", "{", "{")]
        [InlineData("PART { MODULE { X = 3 } } ", "PART", "{", "MODULE", "{", "X = 3", "}", "}")]
        public void SplitLineDataOnBraces_gives_correct_output(string input, params string[] expected)
        {
            var inputLine = new InputLine(1) { Data = input };
            var results = inputLine.SplitLineDataOnBraces().ToArray();

            Assert.Equal(expected.Count(), results.Count());
            for (var i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i], results[i].Data);
            }
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("=", false)]
        [InlineData("A      =", false)]
        [InlineData("B      =           X", true)]
        [InlineData("   C=           Q", true)]
        [InlineData(" D     =Z X Y Z", true)]
        public void IsAttributeDefinition_gives_correct_result(string input, bool expected)
        {
            var line = new InputLine(1) { Data = input };
            var result = line.IsAttributeDefinition();
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", null, null)]
        [InlineData("=", null, null)]
        [InlineData("x=", null, null)]
        [InlineData("=y", null, null)]
        [InlineData("z=q", "z", "q")]
        [InlineData("ax=bc", "ax", "bc")]
        [InlineData("z1    =    q2    ", "z1", "q2")]
        [InlineData("   z2    =    q3", "z2", "q3")]
        public void ToAttributeDefinition_gives_correct_result(string input, string expectedName, string expectedValue)
        {
            var line = new InputLine(1) { Data = input };
            var result = line.ToAttributeDefinition();
            Assert.Equal(expectedName, result?.Name);
            Assert.Equal(expectedValue, result?.Value);
        }
    }
}