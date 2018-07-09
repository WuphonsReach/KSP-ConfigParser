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
                (string Folder, string FileName) info = SplitFilePath(filePath);
                Console.WriteLine($"Folder: {info.Folder}");
                Console.WriteLine($"FileName: {info.FileName}");

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

                    results.AddRange(ConvertNodesToAntennaRecords(
                        info.Folder, 
                        info.FileName,
                        partsWithAntennas
                        ));
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

        private static IEnumerable<Antenna> ConvertNodesToAntennaRecords(
            string directory,
            string fileName,
            IEnumerable<ConfigNode> partsWithAntennas
            )
        {
            var results = new List<Antenna>();
            foreach (var part in partsWithAntennas)
            {
                var antenna = new Antenna
                {
                    Folder = directory,
                    FileName = fileName,
                };

                antenna.Name = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "name")?.Value;
                antenna.Title = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "title")?.Value;
                antenna.Author = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "author")?.Value;
                antenna.Manufacturer = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "manufacturer")?.Value;
                antenna.Category = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "category")?.Value;
                antenna.EntryCost = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "entryCost")?.Value;
                antenna.PartCost = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "cost")?.Value;
                antenna.Mass = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "mass")?.Value;

                var module = part.Nodes.FirstOrDefault(x => 
                    x.Type == NodeType.Module
                    && x.AttributeDefinitions.Any(ad => 
                        ad.Name == "name"
                        && ad.Value == "ModuleDataTransmitter"
                        )
                    );

                antenna.Type = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaType")?.Value;
                antenna.PacketInterval = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetInterval")?.Value;
                antenna.PacketSize = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetSize")?.Value;
                antenna.PacketResourceCost = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetResourceCost")?.Value;
                antenna.RequiredResource = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "requiredResource")?.Value;
                antenna.Power = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaPower")?.Value;
                antenna.OptimumRange = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "optimumRange")?.Value;
                antenna.PacketeFloor = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetFloor")?.Value;
                antenna.PacketCeiling = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetCeiling")?.Value;
                antenna.Combinable = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaCombinable")?.Value;
                antenna.CombinableExponent = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaCombinableExponent")?.Value;

                results.Add(antenna);
            }
            return results;
        }

        private static (string folder, string fileName) SplitFilePath(string filePath)
        {
            var folder = "";
            var fileName = filePath;

            const string gameData = "GameData";
            var gameDataIndex = filePath.IndexOf(gameData);
            if (gameDataIndex >= 0)
                filePath = filePath.Substring(gameDataIndex + gameData.Length);

            //TODO: Use .NET standard methods to break down this file path
            var lastSlashIndex = filePath.LastIndexOfAny(new char[]{ '/', '\\' });
            if (lastSlashIndex > 0) folder = filePath.Substring(0, lastSlashIndex - 1);
            if (lastSlashIndex >= 0) fileName = filePath.Substring(lastSlashIndex + 1);

            return (folder, fileName);
        }
    }
}
