using System.Collections.Generic;

namespace Bg.StateMachine
{
    public class GraphCache
    {
        [System.NonSerialized]
        public List<GraphNode> Nodes = new List<GraphNode>();
        
        public Dictionary<string, GraphNode> NodeDictionary { get; } = new Dictionary<string, GraphNode>();

        public bool TryAddNode(GraphNode node)
        {
            if (NodeDictionary.ContainsKey(node.ID) == false)
            {
                Nodes.Add(node);
                NodeDictionary.Add(node.ID, node);
                return true;
            }

            return false;
        }
    }
}