using System.Collections.Generic;

namespace parse.Models
{
    public class ConfigNode
    {
        public NodeType Type { get; set; }
        public string TypeIdentifier { get; set; }
        public ICollection<ConfigNode> Nodes { get; set; } = new List<ConfigNode>();
        public ICollection<Attribute> Attributes { get; set; }
    }
}