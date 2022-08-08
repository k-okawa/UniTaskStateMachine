using System.Text;
using Bg.UniTaskStateMachine.Editor.SubWindow;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class TransitionIdConstantGenerator : ToolbarElement
    {
        public TransitionIdConstantGenerator(EditorWindow window) : base(window) 
        {
            
        }
        
        public override void OnGUI(Rect rect) 
        {
            if (!Context.IsStateMachineLoaded)
            {
                return;
            }

            if (GUI.Button(new Rect(270, rect.y + 1, 180, 16), 
                    new GUIContent("GenerateTransitionIdConst", "Generate transitionId constant code")))
            {
                var result = GenerateConst();
                CodeGenerationResultWindow.Open("TransitionIdConstant", result);
            }
        }

        private string GenerateConst()
        {
            var transitions = EditorWindow.Context.Graph.Cache.Transitions;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"public static class {EditorWindow.Context.StateMachine.name}Const");
            sb.AppendLine("{");
            foreach (var transition in transitions)
            {
                sb.AppendLine($"    public static readonly string {transition.ID} = \"{transition.ID}\";");
            }
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}