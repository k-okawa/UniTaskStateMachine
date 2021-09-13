using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bg.StateMachine
{
    [System.Serializable]
    public class Graph
    {
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

        public bool TryAddNode(GraphNode node) => cache.TryAddNode(node);

        public bool TryGetNode(string id, out GraphNode node) => cache.TryGetNode(id, out node);

        public bool TryRemoveNode(GraphNode node) => cache.TryRemoveNode(node);

        public bool TryGetTransition(string id, out GraphTransition transition) => cache.TransitionDictionary.TryGetValue(id, out transition);

        public bool TryAddTransition(GraphTransition transition) => cache.TryAddTransition(transition);

        public bool TryRemoveTransition(GraphTransition transition) => cache.TryRemoveTransition(transition);
    }
}