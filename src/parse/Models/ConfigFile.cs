using System.Collections.Generic;

namespace parse.Models
{
    public class ConfigFile
    {
        public string FilePath { get; set; }
        public ConfigNode RootNode { get; set; }
    }
}