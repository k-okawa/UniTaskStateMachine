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

            if (Application.isPlaying) 
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Start", EditorStyles.miniButton, GUILayout.Width(50)))
                    {
                        stateMachine.StateMachine.Start();
                    }

                    if (GUILayout.Button("Stop", EditorStyles.miniButton, GUILayout.Width(50)))
                    {
                        stateMachine.StateMachine.Stop();
                    }
                    
                    if (GUILayout.Button("Pause", EditorStyles.miniButton, GUILayout.Width(50)))
                    {
                        stateMachine.StateMachine.Pause();
                    }
                    
                    if (GUILayout.Button("Resume", EditorStyles.miniButton, GUILayout.Width(60)))
                    {
                        stateMachine.StateMachine.Resume();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}