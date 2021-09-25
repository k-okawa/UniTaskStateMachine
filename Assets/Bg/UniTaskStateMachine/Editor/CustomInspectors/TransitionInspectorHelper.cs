using UnityEngine;
using UnityEditor;

namespace Bg.UniTaskStateMachine.Editor
{
    public class TransitionInspectorHelper : ScriptableObjectSingleton<TransitionInspectorHelper>
    {
        public StateMachineBehaviour StateMachine { get; private set; }
        public GraphTransition Transition { get; private set; }
        public string TransitionID { get; private set; }
        
        public void Inspect(StateMachineBehaviour stateMachine, GraphTransition transition)
        {
            StateMachine = stateMachine;
            TransitionID = transition.ID;
            Transition = transition;

            Selection.activeObject = this;

            var inspectors = Resources.FindObjectsOfTypeAll<TransitionInspector>();

            foreach (var inspector in inspectors)
            {
                inspector.Reload();
            }
        }
    }
}