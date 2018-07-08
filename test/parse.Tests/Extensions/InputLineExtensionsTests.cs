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
            var inputLine = new InputLine { Data = input };
            var results = inputLine.SplitLineDataOnBraces().ToArray();

            Assert.Equal(expected.Count(), results.Count());
            for (var i = 0; i < expected.Count(); i++)
            {
                Assert.Equal(expected[i], results[i].Data);
            }
        }
    }
}