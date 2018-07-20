using System;

namespace SamplesCommon
{
    public static class Common
    {
        /// <summary>Returns a tuple containing the top level folder, the entire folder
        /// path, and the filename where the node was found.  If we are parsing the
        /// ModuleManager file, this will result in no path information unless
        /// we look at the "ParentUrl" attribute.
        /// </summary>
        public static (
            string topFolder,
            string folder,
            string fileName
            ) SplitFilePath(
                string filePath,
                string moduleManagerParentUrl = null
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

            // If we don't have folder information, but we do have ModuleManager ParentUrl, use it
            if (string.IsNullOrWhiteSpace(folder) && !string.IsNullOrWhiteSpace(moduleManagerParentUrl))
            {
                folder = moduleManagerParentUrl;
            }

            var firstSlashIndex = folder.IndexOfAny(new char[]{ '/', '\\' });
            if (firstSlashIndex > 0)
                topFolder = folder.Substring(0, firstSlashIndex);

            return (topFolder, folder, fileName);
        }

        public static void PrintUsage(string name)
        {
            Console.WriteLine();
            Console.WriteLine($"USAGE: {name} filePath [filePath ...]");
            Console.WriteLine();
        }
    }
}
