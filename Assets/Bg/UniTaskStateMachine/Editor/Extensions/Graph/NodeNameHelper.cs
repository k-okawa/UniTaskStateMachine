using System.Collections.Generic;
using System.Text;
using Bg.UniTaskStateMachine.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
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
                stringBuilder.Append(baseName).Append($"({x})");
                res = stringBuilder.ToString();
                x++;
            }

            return res;
        }

        public static bool TryRenameState(this StateMachineBehaviour stateMachine, string oldId, string id)
        {
            var graph = stateMachine.GetStateMachineGraph();

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            if (graph.TryGetNode(id, out _))
            {
                Debug.LogWarningFormat(ErrorMessages.TakenStateID, oldId, id);

                return false;
            }
            
            Undo.RegisterCompleteObjectUndo(stateMachine, "Change node id");
            
            UpdateTransitions(graph, oldId, id);
            UpdateStateName(graph, oldId, id);

            return true;
        }
        
        private static void UpdateTransitions(Graph graph, string oldStateName, string newStateName)
        {
            foreach (var transition in graph.Transitions)
            {
                if (transition.OriginStateID == oldStateName)
                {
                    transition.OriginStateID = newStateName;
                }

                if (transition.TargetStateID == oldStateName)
                {
                    transition.TargetStateID = newStateName;
                }
            }
        }
        
        private static void UpdateStateName(Graph graph, string oldStateName, string newStateName)
        {
            if(graph.TryGetNode(oldStateName, out GraphNode node))
            {
                var state = node as GraphState;

                if(state != null)
                {
                    //Update entry state name of the graph
                    if (graph.EntryStateId == oldStateName)
                    {
                        graph.EntryStateId = newStateName;
                    }

                    //Apply name to state
                    state.ID = newStateName;

                    graph.Cache.RebuildDictionary();
                }
            }
        }
    }
}