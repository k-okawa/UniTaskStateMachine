using System.Linq;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public static class NodeSelectionHelper
    {
        public static GraphNode GetClickedNode(this Graph graph, GraphLayer layer, Vector2 mousePos)
        {
            var reverse = graph.Nodes.Reverse();

            foreach (var node in reverse)
            {
                Rect transformedRect = layer.GetTransformedRect(node.Rect);

                if (transformedRect.Contains(mousePos))
                {
                    return node;
                }
            }

            return null;
        }
    }
}