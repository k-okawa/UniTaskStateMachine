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

        public IList<GraphNode> Nodes => cache.Nodes;

        public bool TryAddNode(GraphNode node) => cache.TryAddNode(node);
    }
}