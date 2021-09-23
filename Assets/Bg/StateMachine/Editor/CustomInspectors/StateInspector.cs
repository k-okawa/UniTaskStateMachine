using System;
using Bg.StateMachine.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Bg.StateMachine.Editor
{
    [CustomEditor(typeof(StateInspectorHelper))]
    public class StateInspector : UnityEditor.Editor
    {
        private SerializedObject serializedStateMachineObject = null;

        private SerializedProperty serializedStateProperty = null;
        private SerializedProperty stateNameProperty = null;
        
        private GUIContent guiContentID = new GUIContent("ID", "A unique ID that can be used to identify the state");
        
        public void OnEnable()
        {
            var inspectorHelper = target as StateInspectorHelper;

            var stateMachine = inspectorHelper.StateMachine;
            var graph = inspectorHelper.Graph;

            if (graph != null)
            {
                serializedStateMachineObject = new SerializedObject(stateMachine);

                var stateArray = serializedStateMachineObject.FindProperty("graph").FindPropertyRelative("states");

                for (int i = 0; i < stateArray.arraySize; i++)
                {
                    stateNameProperty = stateArray.GetArrayElementAtIndex(i).FindPropertyRelative("id");

                    if (stateNameProperty.stringValue == inspectorHelper.StateID)
                    {
                        serializedStateProperty = stateArray.GetArrayElementAtIndex(i);

                        return;
                    }
                }
            }

            Selection.activeObject = null;
        }

        public void Reload()
        {
            OnEnable();
            Repaint();
        }

        public override void OnInspectorGUI()
        {
            if (serializedStateMachineObject == null || serializedStateMachineObject.targetObject == null)
            {
                return;
            }

            var inspectorHelper = target as StateInspectorHelper;
            var graph = (serializedStateMachineObject.targetObject as StateMachineBehaviour).GetStateMachineGraph();

            if (graph.HasNode(inspectorHelper.StateID))
            {
                
            }
        }

        protected override void OnHeaderGUI()
        {
            if (this.serializedStateMachineObject == null || this.serializedStateMachineObject.targetObject == null)
            {
                return;
            }

            var inspectorHelper = this.target as StateInspectorHelper;
            var stateMachine = (serializedStateMachineObject.targetObject as StateMachineBehaviour);
            var graph = stateMachine.GetStateMachineGraph();

            if (graph.TryGetNode(inspectorHelper.StateID, out GraphNode node))
            {
                if (node is GraphState state)
                {
                    bool disabled = EditorApplication.isPlaying || PrefabUtility.IsPartOfAnyPrefab(stateMachine);
                    EditorGUI.BeginDisabledGroup(disabled);
                    EditorGUILayout.Space();

                    string id = state.ID;
                    
                    EditorGUI.BeginChangeCheck();
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUIUtility.labelWidth = 20;
                            id = EditorGUILayout.DelayedTextField(guiContentID, id);
                            EditorGUIUtility.labelWidth = 0;
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (stateMachine.TryRenameState(state.ID, id))
                        {
                            inspectorHelper.Inspect(stateMachine, graph, state);
                        }
                    }
                    
                    
                    EditorGUILayout.Space();

                    var rect = EditorGUILayout.BeginHorizontal();
                    {
                        Handles.color = Color.black;
                        Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();
                    EditorGUI.EndDisabledGroup();
                }
            }
        }
    }
}