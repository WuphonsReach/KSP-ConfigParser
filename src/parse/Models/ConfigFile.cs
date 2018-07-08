using System.Collections.Generic;

namespace parse.Models
{
    public class ConfigFile
    {
        public string FilePath { get; set; }
        public ICollection<ConfigNode> Nodes { get;set; } = new List<ConfigNode>();
    }
}