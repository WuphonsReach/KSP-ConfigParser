using System;
using System.Collections.Generic;
using System.IO;
using parse.Extensions;
using parse.Models;

namespace parse
{
    public class Parser
    {
        public ConfigFile ParseConfigFile(string filePath, Stream stream)
        {
            var result = new ConfigFile
            {
                FilePath = filePath,
            };

            var lines = new List<InputLine>();

            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var input = reader.ReadLine();
                    if (input != null)
                    {
                        var line = input.ParseLine();
                        if (!string.IsNullOrWhiteSpace(line.Data))
                        {
                            // We have an interesting line with data
                            var candidateData = line.Data.Trim();
                            // open/close curly braces should be their own element
                            // so if we find such a situation, we need to break the
                            // line apart
                        }
                    }
                }
            }

            return result;
        }
    }
}
