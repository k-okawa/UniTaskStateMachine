using System.Collections.Generic;
using System.Text;
using Bg.UniTaskStateMachine.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public static class TransitionNameHelper
    {
        public static bool TryRenameTransition(this StateMachineBehaviour stateMachine, GraphTransition transition, string id)
        {
            var graph = stateMachine.GetStateMachineGraph();

            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            else if (graph.TryGetTransition(id, out _))
            {
                Debug.LogWarningFormat(ErrorMessages.TakenTransitionName, transition.ID, id);
                return false;
            }
            else
            {
                Undo.RegisterCompleteObjectUndo(stateMachine, "Change transition name");

                transition.ID = id;

                graph.Cache.RebuildDictionary();

                return true;
            }
        }

        public static string GetUniqueTransitionId(this Graph graph)
        {
            int x = 1;

            string res = "Transition";
            
            var stringBuilder = new StringBuilder();

            while (graph.TryGetTransition(res, out _))
            {
                stringBuilder.Clear();
                stringBuilder.Append("Transition").Append($"({x})");
                res = stringBuilder.ToString();
                x++;
            }

            return res;
        }
    }
}