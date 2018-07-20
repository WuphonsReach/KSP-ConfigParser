using FileHelpers;
using parse;
using parse.Extensions;
using parse.Models;
using SamplesCommon;
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

                    results.AddRange(ConvertNodesToExportRecords(
                        filePath,
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

                var command = part.Nodes.FirstOrDefault(x => 
                    x.Type == NodeType.Module
                    && x.AttributeDefinitions.Any(ad => 
                        ad.Name == "name"
                        && ad.Value == "ModuleCommand"
                        )
                    );
                record.MinimumCrew = command?.AttributeDefinitions.FirstOrDefault(x => x.Name == "minimumCrew")?.Value;
                record.CrewCapacity = part.AttributeDefinitions.FirstOrDefault(x => x.Name == "CrewCapacity")?.Value;

                var sas = part.Nodes.FirstOrDefault(x => 
                    x.Type == NodeType.Module
                    && x.AttributeDefinitions.Any(ad => 
                        ad.Name == "name"
                        && ad.Value == "ModuleSAS"
                        )
                    );
                record.SasServiceLevel = sas?.AttributeDefinitions.FirstOrDefault(x => x.Name == "SASServiceLevel")?.Value;

                var ec = part.Nodes.FirstOrDefault(x => 
                    x.Type == NodeType.Resource
                    && x.AttributeDefinitions.Any(ad => 
                        ad.Name == "name"
                        && ad.Value == "ElectricCharge"
                        )
                    );
                record.ElectricCharge = ec?.AttributeDefinitions.FirstOrDefault(x => x.Name == "maxAmount")?.Value;

                var reactionWheel = part.Nodes.FirstOrDefault(x => 
                    x.Type == NodeType.Module
                    && x.AttributeDefinitions.Any(ad => 
                        ad.Name == "name"
                        && ad.Value == "ModuleReactionWheel"
                        )
                    );
                record.ReactionWheelPitchTorque = reactionWheel?.AttributeDefinitions.FirstOrDefault(x => x.Name == "PitchTorque")?.Value;
                record.ReactionWheelYawTorque = reactionWheel?.AttributeDefinitions.FirstOrDefault(x => x.Name == "YawTorque")?.Value;
                record.ReactionWheelRollTorque = reactionWheel?.AttributeDefinitions.FirstOrDefault(x => x.Name == "RollTorque")?.Value;

                results.Add(record);
            }
            return results;
        }
    }
}
