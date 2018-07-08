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

            var lines = new List<string>();

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
                            lines.Add(line.Data.Trim());
                        }
                    }
                }
            }

            return result;
        }
    }
}
