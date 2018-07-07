using System.Collections.Generic;

namespace parse.Models
{
    public class ConfigFile
    {
        public string FilePath { get; set; }
        public ICollection<Node> Nodes { get;set; } = new List<Node>();
    }
}