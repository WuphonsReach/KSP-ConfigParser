using Xunit;
using parse;
using System.Collections.Generic;
using parse.Models;
using System;

namespace parse.Tests.ParserMethodTests
{
    public class IdentifyBlocksTests
    {
        private Parser _sut = new Parser();

        private void AssertCorrectBlockId(int expected, IList<InputLine> inputs, int lineNumber)
        {
            var line = inputs[lineNumber-1];
            Assert.True(
                expected == line.BlockId,
                $"expected id='{expected}' got: line[{lineNumber-1}]='{line.Data}', id='{line.BlockId}', depth='{line.BlockDepth}'"
                );
        }

        private void AssertCorrectBlockDepth(int expected, IList<InputLine> inputs, int lineNumber)
        {
            var line = inputs[lineNumber-1];
            Assert.True(
                expected == line.BlockDepth,
                $"expected depth='{expected}' got: line[{lineNumber-1}]='{line.Data}', id='{line.BlockId}', depth='{line.BlockDepth}'"
                );
        }

        private IList<InputLine> Clone(IList<InputLine> lines)
        {
            var clone = new List<InputLine>();
            foreach(var line in lines)
            {
                clone.Add(new InputLine(line.RawLineNumber){ Data = line.Data });
            }
            return clone;
        }

        private readonly IList<InputLine> _simpleBlock = new List<InputLine>{
                new InputLine(1) { Data = "PART" },
                new InputLine(2) { Data = "{" },
                new InputLine(3) { Data = "x = y" },
                new InputLine(4) { Data = "a = b" },
                new InputLine(5) { Data = "}" },
                new InputLine(6) { Data = "" },
            };

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(3, 1, 1)]
        [InlineData(4, 1, 1)]
        [InlineData(5, 1, 1)]
        [InlineData(6, 0, 0)]
        public void IdentifyBlocks_SimpleBlock_gives_correct_id_and_depth(int rawLineNumber, int expectedId, int expectedDepth)
        {
            var inputs = _simpleBlock;
            _sut.IdentifyBlocks(inputs);
            AssertCorrectBlockId(expectedId, inputs, rawLineNumber);
            AssertCorrectBlockDepth(expectedDepth, inputs, rawLineNumber);
        }

        private readonly IList<InputLine> _blockWithTwoNodes = new List<InputLine>{
                new InputLine(1) { Data = "PART" },
                new InputLine(2) { Data = "{" },
                new InputLine(3) { Data = "x = y" },
                new InputLine(4) { Data = "MODULE" },
                new InputLine(5) { Data = "{" },
                new InputLine(6) { Data = "v = x" },
                new InputLine(7) { Data = "}" },
                new InputLine(8) { Data = "a = b" },
                new InputLine(9) { Data = "MODULE" },
                new InputLine(10) { Data = "{" },
                new InputLine(11) { Data = "qrz = abc" },
                new InputLine(12) { Data = "}" },
                new InputLine(13) { Data = "}" },
                new InputLine(14) { Data = "" },
            };

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1, 1)]
        [InlineData(3, 1, 1)]
        [InlineData(4, 2, 2)]
        [InlineData(5, 2, 2)]
        [InlineData(6, 2, 2)]
        [InlineData(7, 2, 2)]
        [InlineData(8, 1, 1)]
        [InlineData(9, 3, 2)]
        [InlineData(10, 3, 2)]
        [InlineData(11, 3, 2)]
        [InlineData(12, 3, 2)]
        [InlineData(13, 1, 1)]
        [InlineData(14, 0, 0)]
        public void IdentifyBlocks_blockWithTwoNodes_gives_correct_id_and_depth(int rawLineNumber, int expectedId, int expectedDepth)
        {
            var inputs = _blockWithTwoNodes;
            _sut.IdentifyBlocks(inputs);
            AssertCorrectBlockId(expectedId, inputs, rawLineNumber);
            AssertCorrectBlockDepth(expectedDepth, inputs, rawLineNumber);
        }

    }
}