using FileHelpers;
using parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AntennaBalanceValues
{
    public class Program
    {
        public const string Name = "AntennaBalanceValues";

        static void Main(string[] args)
        {
            var filePaths = args?.ToList();
            if (filePaths == null || filePaths.Count == 0) 
                filePaths = new List<string>{
                    "/home/syb/WR/ksp/1.4.4/Play/GameData/ModuleManager.ConfigCache"
                    };

            Console.WriteLine($"{Name}:");
            Console.WriteLine();

            var results = new List<Antenna>();
            var parser = new Parser();

            foreach(var filePath in filePaths)
            {
                Console.WriteLine($"Path: {filePath}");
                using (var stream = File.Open(filePath, FileMode.Open))
                {
                    Console.WriteLine($"Length: {stream.Length}");
                    var configFile = parser.ParseConfigFile(filePath, stream);
                }                
                Console.WriteLine();
            }

            var engine = new FileHelperEngine<Antenna>();
            

        }
    }
}
