using System.Collections.Generic;
using System.Linq;

namespace Bg.UniTaskStateMachine
{
    public class GraphCache
    {
        [System.NonSerialized]
        public List<GraphTransition> Transitions = new List<GraphTransition>();
        
        [System.NonSerialized]
        public List<GraphNode> Nodes = new List<GraphNode>();
        
        public Dictionary<string, GraphTransition> TransitionDictionary { get; } = new Dictionary<string, GraphTransition>();
        
        public Dictionary<string, GraphNode> NodeDictionary { get; } = new Dictionary<string, GraphNode>();

        public void SerializeCache(Graph graph)
        {
            Graph.SerializedData serializedData = new Graph.SerializedData(graph);
            
            serializedData.States.Clear();
            foreach (var node in Nodes)
            {
                if (node is GraphState state)
                {
                    serializedData.States.Add(state);
                }
            }
            
            serializedData.Transitions.Clear();
            foreach (var transition in Transitions)
            {
                serializedData.Transitions.Add(transition);
            }
        }

        public void RebuildDictionary()
        {
            BuildNodeDictionary();
            BuildTransitionDictionary();
        }

        public void BuildCache(Graph graph)
        {
            Graph.SerializedData serializedData = new Graph.SerializedData(graph);
            
            CacheNodes(serializedData);
            CacheTransitions(serializedData);
            BuildNodeDictionary();
            BuildTransitionDictionary();
        }

        void CacheNodes(Graph.SerializedData serializedData)
        {
            Nodes.Clear();

            foreach (var node in serializedData.States)
            {
                Nodes.Add(node);
            }
        }

        void CacheTransitions(Graph.SerializedData serializedData)
        {
            Transitions.Clear();

            foreach (var transition in serializedData.Transitions)
            {
                Transitions.Add(transition);
            }
        }

        void BuildNodeDictionary()
        {
            NodeDictionary.Clear();

            foreach (var node in Nodes)
            {
                if (NodeDictionary.ContainsKey(node.ID) == false)
                {
                    NodeDictionary.Add(node.ID, node);
                }
            }
        }

        void BuildTransitionDictionary()
        {
            foreach (var transition in Transitions)
            {
                if (TransitionDictionary.ContainsKey(transition.ID) == false)
                {
                    TransitionDictionary.Add(transition.ID, transition);
                }
            }
        }

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
        
        public bool TryGetState(string id, out GraphState state)
        {
            if (TryGetNode(id, out GraphNode node) && node is GraphState)
            {
                state = node as GraphState;
                return true;
            }
            else
            {
                state = null;
                return false;
            }
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

        public bool TryRemoveTransition(GraphTransition transition)
        {
            bool success = false;

            if (TransitionDictionary.ContainsKey(transition.ID))
            {
                TransitionDictionary.Remove(transition.ID);
                success = true;
            }

            if (Transitions.Contains(transition))
            {
                Transitions.Remove(transition);
                success = true;
            }

            return success;
        }
        
        public bool HasTransition(string id)
        {
            return TransitionDictionary.ContainsKey(id);
        }

        public bool HasNode(string id)
        {
            return NodeDictionary.ContainsKey(id);
        }

        public bool HasState(string id)
        {
            return TryGetState(id, out _);
        }
    }
}