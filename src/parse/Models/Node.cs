using System.Collections.Generic;

namespace parse.Models
{
    public class Node
    {
        public NodeType Type { get; set; }
        public string TypeIdentifier { get; set; }
        public ICollection<Node> Nodes { get; set; } = new List<Node>();
        public ICollection<Attribute> Attributes { get; set; }
    }
}