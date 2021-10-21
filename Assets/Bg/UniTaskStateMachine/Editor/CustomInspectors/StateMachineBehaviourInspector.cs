using System;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    [CustomEditor(typeof(StateMachineBehaviour))]
    public class StateMachineBehaviourInspector : UnityEditor.Editor
    {
        private StateMachineBehaviour stateMachine = null;

        private void OnEnable()
        {
            stateMachine = target as StateMachineBehaviour;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (stateMachine == null)
            {
                return;
            }
            
            DrawEditorButton();
        }

        private void DrawEditorButton()
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Graph Editor");

                if (GUILayout.Button("Open", EditorStyles.miniButton, GUILayout.Width(50)))
                {
                    EditorWindow window = (EditorWindow) UnityEditor.EditorWindow.GetWindow(typeof(EditorWindow), false, "State Machine Graph", true);
                    window.Context.LoadStateMachine(stateMachine);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}