using System;
using System.IO;
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

            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                }
            }

            return result;
        }
    }
}
