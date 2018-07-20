using FileHelpers;
using parse;
using parse.Extensions;
using parse.Models;
using SamplesCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProbeCoreValues
{
    public class Program
    {
        public const string Name = "AntennaBalanceValues";

        static void Main(string[] args)
        {
            var filePaths = args?.ToList();
            if (filePaths == null || filePaths.Count == 0) 
            {
                Common.PrintUsage(Name);
                return;
            }

            Console.WriteLine($"{Name}:");
            Console.WriteLine();

            var results = new List<ExportRecord>();
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

                    results.AddRange(ConvertNodesToExportRecords(
                        filePath,
                        partsWithAntennas
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

        private static IEnumerable<ExportRecord> ConvertNodesToExportRecords(
            string filePath,
            IEnumerable<ConfigNode> partsWithAntennas
            )
        {
            var results = new List<ExportRecord>();
            foreach (var part in partsWithAntennas)
            {
                (string TopFolder, string Folder, string FileName) info = Common.SplitFilePath(filePath);
                if (string.IsNullOrWhiteSpace(info.Folder))
                {
                    var parent = part.Parent;
                    if (parent?.Type == NodeType.UrlConfig)
                    {
                        var parentUrl = parent?.AttributeDefinitions.FirstOrDefault(x => x.Name == "parentUrl")?.Value;
                        info = Common.SplitFilePath(filePath, parentUrl);
                    }
                }

                var record = new ExportRecord
                {
                    TopFolder = info.TopFolder,
                    Folder = info.Folder,
                    FileName = info.FileName,
                };

                record.Name = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "name")?.Value;
                record.Title = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "title")?.Value;
                record.Author = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "author")?.Value;
                record.Manufacturer = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "manufacturer")?.Value;
                record.Category = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "category")?.Value;
                record.EntryCost = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "entryCost")?.Value;
                record.PartCost = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "cost")?.Value;
                record.Mass = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "mass")?.Value;

                var module = part.Nodes.FirstOrDefault(x => 
                    x.Type == NodeType.Module
                    && x.AttributeDefinitions.Any(ad => 
                        ad.Name == "name"
                        && ad.Value == "ModuleDataTransmitter"
                        )
                    );

                record.Type = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaType")?.Value;
                record.PacketInterval = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetInterval")?.Value;
                record.PacketSize = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetSize")?.Value;
                record.PacketResourceCost = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetResourceCost")?.Value;
                record.RequiredResource = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "requiredResource")?.Value;
                record.Power = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaPower")?.Value;
                record.OptimumRange = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "optimumRange")?.Value;
                record.PacketeFloor = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetFloor")?.Value;
                record.PacketCeiling = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "packetCeiling")?.Value;
                record.Combinable = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaCombinable")?.Value;
                record.CombinableExponent = module?.AttributeDefinitions.FirstOrDefault(x => x.Name == "antennaCombinableExponent")?.Value;

                results.Add(record);
            }
            return results;
        }
    }
}
