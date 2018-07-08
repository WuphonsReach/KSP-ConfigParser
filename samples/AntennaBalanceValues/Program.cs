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
                    "~/WR/ksp/1.4.4/Play/GameData/ModuleManager.ConfigCache"
                    };

            Console.WriteLine(
                $"{Name}: {string.Join(",", filePaths)}"
                );
            Console.WriteLine();

            var results = new List<Antenna>();

            foreach(var filePath in filePaths)
            {
                FileStream stream = File.Open(filePath, FileMode.Open);
                
            }

            var engine = new FileHelperEngine<Antenna>();
            

        }
    }
}
