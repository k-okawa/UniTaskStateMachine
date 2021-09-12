using System.Collections.Generic;
using System.Linq;

namespace Bg.StateMachine
{
    public class GraphCache
    {
        [System.NonSerialized]
        public List<GraphTransition> Transitions = new List<GraphTransition>();
        
        [System.NonSerialized]
        public List<GraphNode> Nodes = new List<GraphNode>();
        
        public Dictionary<string, GraphTransition> TransitionDictionary { get; } = new Dictionary<string, GraphTransition>();
        
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

        public bool TryGetNode(string id, out GraphNode node)
        {
            return NodeDictionary.TryGetValue(id, out node);
        }

        public bool TryAddTransition(GraphTransition transition)
        {
            if (!TransitionDictionary.ContainsKey(transition.ID))
            {
                Transitions.Add(transition);
                TransitionDictionary.Add(transition.ID, transition);
                return true;
            }

            return false;
        }

        public bool TryRemoveNode(GraphNode node)
        {
            if (NodeDictionary.ContainsKey(node.ID))
            {
                Nodes.Remove(node);
                NodeDictionary.Remove(node.ID);
                return true;
            }

            return false;
        }
    }
}