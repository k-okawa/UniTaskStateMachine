using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bg.UniTaskStateMachine
{
    [System.Serializable]
    public class Graph : ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<GraphState> states = new List<GraphState>();
        
        [SerializeField]
        private List<GraphTransition> transitions = new List<GraphTransition>();
        
        [SerializeField]
        private Preferences preferences = new Preferences();
        
        [NonSerialized]
        private GraphCache cache = new GraphCache();
        
        [SerializeField] 
        private string entryState = string.Empty;

        public string EntryStateId
        {
            get => this.entryState;
            set => this.entryState = value;
        }

        public IList<GraphNode> Nodes => cache.Nodes;

        public IList<GraphTransition> Transitions => cache.Transitions;

        public Preferences Preferences => preferences;

        public GraphCache Cache => cache;

        public struct SerializedData
        {
            public SerializedData(Graph graph)
            {
                States = graph.states;
                Transitions = graph.transitions;
            }

            public List<GraphState> States { get; private set; }
            public List<GraphTransition> Transitions { get; private set; }
        }
        
        public void OnBeforeSerialize()
        {
            cache.SerializeCache(this);
        }

        public void OnAfterDeserialize()
        {
            cache.BuildCache(this);
        }

        public bool TryAddNode(GraphNode node) => cache.TryAddNode(node);

        public bool TryGetNode(string id, out GraphNode node) => cache.TryGetNode(id, out node);

        public bool TryRemoveNode(GraphNode node) => cache.TryRemoveNode(node);

        public bool TryGetTransition(string id, out GraphTransition transition) => cache.TransitionDictionary.TryGetValue(id, out transition);

        public bool TryAddTransition(GraphTransition transition) => cache.TryAddTransition(transition);

        public bool TryRemoveTransition(GraphTransition transition) => cache.TryRemoveTransition(transition);
        
        public bool HasTransition(string id) => cache.HasTransition(id);
        
        public bool HasNode(string id) => cache.HasNode(id);
        
        public bool HasState(string id) => cache.HasState(id);
    }
}