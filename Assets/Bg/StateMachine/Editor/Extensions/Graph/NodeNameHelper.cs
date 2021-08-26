using System.Collections.Generic;
using System.Text;

namespace Bg.StateMachine.Editor
{
    public static class NodeNameHelper
    {
        public static string GetUniqueStateName(this Graph graph)
        {
            return GetUniqueNodeName(graph.Nodes, "State");
        }
        
        private static string GetUniqueNodeName<TValue>(IList<TValue> nodes, string baseName) where TValue : GraphNode
        {
            int x = 1;

            string res = baseName;
            
            var stringBuilder = new StringBuilder();
            
            Dictionary<string, GraphNode> dictionary = new Dictionary<string, GraphNode>();

            foreach (GraphNode node in nodes)
            {
                dictionary.Add(node.ID, node);
            }

            while (dictionary.ContainsKey(res))
            {
                stringBuilder.Clear();
                stringBuilder.Append(baseName).Append(" ").Append(x);
                res = stringBuilder.ToString();
                x++;
            }

            return res;
        }
    }
}