using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class StateInspectorHelper : ScriptableObjectSingleton<StateInspectorHelper>
    {
        public string StateID { get; private set; }
        public Graph Graph { get; private set; }
        public StateMachineBehaviour StateMachine { get; private set; }

        public void Inspect(StateMachineBehaviour stateMachine, Graph graph, GraphState state)
        {
            StateMachine = stateMachine;
            Graph = graph;
            StateID = state.ID;

            Selection.activeObject = this;

            var inspectors = Resources.FindObjectsOfTypeAll<StateInspector>();

            foreach (var inspector in inspectors)
            {
                inspector.Reload();
            }
        }
    }
}