using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class StateMachineField : ToolbarElement
    {
        public StateMachineField(EditorWindow window) : base(window)
        {
            Width = 248;
        }

        public override void OnGUI(Rect rect)
        {
            var fieldRect = new Rect(2, rect.y + 1, this.Width, 16);
            
            EditorGUI.BeginChangeCheck();
            {
                StateMachineBehaviour stateMachine = (StateMachineBehaviour)EditorGUI.ObjectField(fieldRect, Context.StateMachine, typeof(StateMachineBehaviour), true);

                if (EditorGUI.EndChangeCheck())
                {
                    Context.LoadStateMachine(stateMachine);
                }
            }
        }
    }
}