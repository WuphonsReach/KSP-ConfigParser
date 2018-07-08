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

        public ConfigNode ParseBlocksIntoNodes(IList<InputLine> fileLines)
        {
            var root = new ConfigNode();
            var currentNode = root;
            var nodeStack = new Stack<ConfigNode>();
            var currentBlockId = -1;
            var currentDepth = -1;

            foreach(var line in fileLines)
            {
                if (line.BlockDepth > currentDepth) // go deeper
                {
                    nodeStack.Push(currentNode);
                    currentNode = new ConfigNode
                    {
                        Type = line.Data.ToEnumOrDefault<NodeType>(),
                        TypeIdentifier = line.Data,
                        Parent = nodeStack.Peek(),
                    };
                    nodeStack.Peek().Nodes.Add(currentNode);
                }
                else if (line.BlockDepth < currentDepth) // go shallower
                {
                    currentNode = nodeStack.Pop();
                }
                else if (line.BlockId != currentBlockId) // new block at same level
                {
                    currentNode = new ConfigNode
                    {
                        Type = line.Data.ToEnumOrDefault<NodeType>(),
                        TypeIdentifier = line.Data,
                    };
                    nodeStack.Peek().Nodes.Add(currentNode);
                }

                currentNode.InputLines.Add(line);
                if (line.IsAttributeDefinition()) 
                    currentNode.AttributeDefinitions.Add(line.ToAttributeDefinition());

                currentBlockId = line.BlockId;
                currentDepth = line.BlockDepth;
            }
            
            return root;
        }

        public void IdentifyBlocks(IList<InputLine> fileLines)
        {
            var idStack = new Stack<int>();
            int blockId = 0;
            int runningBlockId = 0;
            int depth = 0;
            for(var i = 0; i < fileLines.Count; i++)
            {
                fileLines[i].BlockId = blockId;
                fileLines[i].BlockDepth = depth;

                if (IsOpeningBrace(fileLines[i].Data))
                {
                    idStack.Push(blockId);
                    runningBlockId++;
                    blockId = runningBlockId;
                    depth++;

                    MarkBlockIdentifier(i, fileLines, runningBlockId, idStack.Peek(), depth);
                }

                if (IsClosingBrace(fileLines[i].Data))
                {
                    depth--;
                    blockId = idStack.Pop();

                    if (depth < 0){
                        var e = new Exception("Found too many closing braces!");
                        e.Data["rawLineNumber"] = fileLines[i].RawLineNumber;
                        e.Data["blockId"] = blockId;
                    }
                }
            }

            //TODO: Throw exception here if we don't end up back at depth zero?
        }

        ///<summary>Walk backwards in previous block/depth to find block identifier</summary>
        private void MarkBlockIdentifier(int openingBraceIndex, IList<InputLine> fileLines, int blockId, int parentBlockId, int depth)
        {
            for (var i = openingBraceIndex; i >= 0; i--)
            {
                if (fileLines[i].BlockDepth != depth - 1) return;
                if (fileLines[i].BlockId != parentBlockId) return;

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
