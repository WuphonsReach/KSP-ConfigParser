using FileHelpers;
using parse;
using parse.Extensions;
using parse.Models;
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

                    var nodes = configFile.RootNode.Descendants();
                    var partNodes = nodes.Where(x => x.Type == NodeType.Part).ToList();
                    Console.WriteLine(
                        $"Part nodes: {partNodes.Count}"
                        );

                    var partsWithAntennas = partNodes
                        .Where(x => x.Nodes
                            .Any(n => 
                                n.Type == NodeType.Module
                                && n.AttributeDefinitions.Any(ad => 
                                    ad.Name == "name"
                                    && ad.Value == "ModuleDataTransmitter"
                                    )
                                )
                            )
                        ;
                    Console.WriteLine(
                        $"Part nodes with antennas: {partsWithAntennas.Count()}"
                        );

                    results.AddRange(ConvertNodesToAntennaRecords(partsWithAntennas));
                }                
                Console.WriteLine($"Finished {filePath}");
                Console.WriteLine();
            }

            Console.WriteLine("Writing output.csv");
            var engine = new FileHelperEngine<Antenna>();
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile("output.csv", results);
            Console.WriteLine("Completed.");
            Console.WriteLine();
        }

        private static IEnumerable<Antenna> ConvertNodesToAntennaRecords(IEnumerable<ConfigNode> partsWithAntennas)
        {
            var results = new List<Antenna>();
            foreach (var part in partsWithAntennas)
            {
                


            }
            return results;
        }
    }
}
