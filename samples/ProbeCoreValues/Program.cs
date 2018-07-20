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
        public const string Name = "ProbeCoreValues";

        static void Main(string[] args)
        {
            var filePaths = args?.ToList();
            if (filePaths == null || filePaths.Count == 0) 
                filePaths = new List<string>{
                    "/home/syb/WR/ksp/1.4.4/Play/GameData/ModuleManager.ConfigCache"
                    };

            Console.WriteLine($"{Name}:");
            Console.WriteLine();

            var results = new List<ExportRecord>();
            var parser = new Parser();

            foreach(var filePath in filePaths)
            {
                Console.WriteLine($"Path: {filePath}");
                (string TopFolder, string Folder, string FileName) info = SplitFilePath(filePath);
                Console.WriteLine($"TopFolder: {info.TopFolder}");
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

                    var partsWithModuleCommand = partNodes
                        .Where(x => x.Nodes
                            .Any(n => 
                                n.Type == NodeType.Module
                                && n.AttributeDefinitions.Any(ad => 
                                    ad.Name == "name"
                                    && ad.Value == "ModuleCommand"
                                    )
                                )
                            )
                        ;

                    Console.WriteLine(
                        $"Part nodes with ModuleCommand: {partsWithModuleCommand.Count()}"
                        );

                    results.AddRange(ConvertNodesToAntennaRecords(
                        info.TopFolder,
                        info.Folder, 
                        info.FileName,
                        partsWithModuleCommand
                        ));
                }                
                Console.WriteLine($"Finished {filePath}");
                Console.WriteLine();
            }

            Console.WriteLine($"Writing {Name}.csv");
            var engine = new FileHelperEngine<ExportRecord>();
            engine.HeaderText = engine.GetFileHeader();
            engine.WriteFile($"{Name}.csv", results);
            Console.WriteLine("Completed.");
            Console.WriteLine();
        }

        private static IEnumerable<ExportRecord> ConvertNodesToAntennaRecords(
            string topDirectory,
            string directory,
            string fileName,
            IEnumerable<ConfigNode> partsWithAntennas
            )
        {
            var results = new List<ExportRecord>();
            foreach (var part in partsWithAntennas)
            {
                var record = new ExportRecord
                {
                    TopFolder = topDirectory,
                    Folder = directory,
                    FileName = fileName,
                };

                record.Name = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "name")?.Value;
                record.Title = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "title")?.Value;
                record.Author = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "author")?.Value;
                record.Manufacturer = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "manufacturer")?.Value;
                record.Category = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "category")?.Value;
                record.EntryCost = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "entryCost")?.Value;
                record.PartCost = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "cost")?.Value;
                record.Mass = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "mass")?.Value;

                var antenna = part.Nodes.FirstOrDefault(x => 
                    x.Type == NodeType.Module
                    && x.AttributeDefinitions.Any(ad => 
                        ad.Name == "name"
                        && ad.Value == "ModuleDataTransmitter"
                        )
                    );

                record.Type = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaType")?.Value;
                record.PacketInterval = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetInterval")?.Value;
                record.PacketSize = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetSize")?.Value;
                record.PacketResourceCost = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetResourceCost")?.Value;
                record.RequiredResource = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "requiredResource")?.Value;
                record.Power = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaPower")?.Value;
                record.OptimumRange = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "optimumRange")?.Value;
                record.PacketeFloor = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetFloor")?.Value;
                record.PacketCeiling = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetCeiling")?.Value;
                record.Combinable = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaCombinable")?.Value;
                record.CombinableExponent = antenna?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaCombinableExponent")?.Value;

                results.Add(record);
            }
            return results;
        }

        private static (string topFolder, string folder, string fileName) SplitFilePath(
            string filePath
            )
        {
            var topFolder = "";
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

            var firstFolderSlashIndex = folder.IndexOfAny(new char[]{ '/', '\\' });
            if (firstFolderSlashIndex > 0) topFolder = folder.Substring(0, firstFolderSlashIndex - 1);

            return (topFolder, folder, fileName);
        }
    }
}
