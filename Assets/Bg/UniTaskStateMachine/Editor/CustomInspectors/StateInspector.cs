using System;
using System.Collections.Generic;
using System.Linq;
using Bg.UniTaskStateMachine.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    [CustomEditor(typeof(StateInspectorHelper))]
    public class StateInspector : UnityEditor.Editor
    {
        private SerializedObject serializedStateMachineObject = null;

        private SerializedProperty serializedStateProperty = null;
        private SerializedProperty stateNameProperty = null;
        
        private List<string> stateComponentNames = new List<string>();
        
        private GUIContent guiContentID = new GUIContent("ID", "A unique ID that can be used to identify the state");
        private GUIContent guiComponent = new GUIContent("StateComponentMenu", "State component names attached to GameObject that has StateMachineBehaviour are dropped down");

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
            var stateMachine = (serializedStateMachineObject.targetObject as StateMachineBehaviour);
            var graph = (serializedStateMachineObject.targetObject as StateMachineBehaviour).GetStateMachineGraph();

            if (graph.HasNode(inspectorHelper.StateID))
            {
                serializedStateMachineObject.Update();
                
                EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
                {
                    // StateComponentName
                    EditorGUILayout.LabelField(guiComponent);
                    CollectStateComponent();
                    var stateComponentProperty = serializedStateProperty.FindPropertyRelative("stateComponent");
                    var selectedComponent = stateComponentProperty.objectReferenceValue as BaseStateComponent;
                    int selectedIndex = 0;
                    if (selectedComponent != null)
                    {
                        var selected = stateComponentNames.Select((name, index) => new {name, index})
                            .FirstOrDefault(itr => itr.name == selectedComponent.GetType().Name);
                        selectedIndex = selected?.index ?? 0;
                    }

                    int popupIndex = EditorGUILayout.Popup(selectedIndex, stateComponentNames.ToArray());
                    var setComponent = stateMachine.GetComponent(stateComponentNames[popupIndex]);
                    stateComponentProperty.objectReferenceValue = setComponent;
                }
                EditorGUI.EndDisabledGroup();

                serializedStateMachineObject.ApplyModifiedProperties();
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

        void CollectStateComponent()
        {
            stateComponentNames.Clear();
            stateComponentNames.Add("None");
            
            var inspectorHelper = target as StateInspectorHelper;
            var stateMachine = (serializedStateMachineObject.targetObject as StateMachineBehaviour);

            if (inspectorHelper == null || stateMachine == null)
            {
                return;
            }
            
            var stateComponents = stateMachine.GetComponents<BaseStateComponent>();
            foreach (var stateComponent in stateComponents)
            {
                stateComponentNames.Add(stateComponent.GetType().Name);
            }
        }
    }
}