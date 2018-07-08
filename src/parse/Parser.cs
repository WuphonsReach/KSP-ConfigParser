using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using parse.Extensions;
using parse.Models;

namespace parse
{
    public class Parser
    {
        public ConfigFile ParseConfigFile(string filePath, Stream stream)
        {
            var fileLines = ReadStreamIntoInputLines(filePath, stream);
            var result = new ConfigFile
            {
                FilePath = filePath,
            };

            result.RootNode = ParseInputLinesIntoNodes(fileLines);

            return result;
        }

        private ConfigNode ParseInputLinesIntoNodes(IList<InputLine> fileLines)
        {
            var results = new ConfigNode();
            if (fileLines == null) return results;

            IdentifyBlocks(fileLines);
            results = ParseBlocksIntoNodes(fileLines);
            return results;
        }

        private List<ConfigNode> ParseBlocksIntoNodes(IList<InputLine> fileLines)
        {
            throw new NotImplementedException();
        }

        public void IdentifyBlocks(IList<InputLine> fileLines)
        {
            int blockId = 0;
            int depth = 0;
            for(var i = 0; i <= fileLines.Count; i++)
            {
                if (IsOpeningBrace(fileLines[i].Data))
                {
                    blockId++;
                    depth++;

                    MarkBlockIdentifier(i, fileLines, blockId, depth);
                }

                if (IsClosingBrace(fileLines[i].Data))
                {
                    depth--;
                }

                fileLines[i].BlockId = blockId;
                fileLines[i].BlockDepth = depth;
            }
        }

        ///<summary>Walk backwards in previous block/depth to find block identifier</summary>
        private void MarkBlockIdentifier(int openingBraceIndex, IList<InputLine> fileLines, int blockId, int depth)
        {
            for (var i = openingBraceIndex - 1; i >= 0; i--)
            {
                if (fileLines[i].BlockDepth != depth - 1) return;
                if (fileLines[i].BlockId != blockId - 1) return;

                fileLines[i].BlockDepth = depth;
                fileLines[i].BlockId = blockId;

                const string blockIdentifierRegexPattern = @"^[a-zA-Z0-9_-]+$";
                Regex blockIdentifierRegex = new Regex(blockIdentifierRegexPattern);
                if (blockIdentifierRegex.Match(fileLines[i].Data).Success)
                {
                    return;
                }
            }
        }

        private bool IsOpeningBrace(string line)
        {
            return (line == Constants.OpeningBrace);
        }

        private bool IsClosingBrace(string line)
        {
            return (line == Constants.ClosingBrace);
        }

        private IList<InputLine> ReadStreamIntoInputLines(string filePath, Stream stream)
        {
            int rawLineCounter = 1;
            var lines = new List<InputLine>();

            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var input = reader.ReadLine();
                    if (input != null)
                    {
                        var line = input.ParseLine(rawLineCounter);
                        if (!string.IsNullOrWhiteSpace(line.Data))
                        {
                            // We have an interesting line with data
                            var candidates = line.SplitLineDataOnBraces();
                            lines.AddRange(candidates);
                        }
                    }
                    rawLineCounter++;
                }
            }

            return lines;        
        }
    }
}
