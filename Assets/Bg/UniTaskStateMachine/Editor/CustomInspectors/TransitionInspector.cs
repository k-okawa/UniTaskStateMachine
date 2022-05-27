using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bg.UniTaskStateMachine.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    [CustomEditor(typeof(TransitionInspectorHelper))]
    public class TransitionInspector : UnityEditor.Editor
    {
        private SerializedObject serializedStateMachineObject = null;

        private SerializedProperty conditionMethodNameProperty = null;
        private SerializedProperty isNegativeProperty = null;
        
        private List<string> conditionMethodNames = new List<string>();
        
        private GUIContent guiContentDesc = new GUIContent("Desc", "Transition description");
        private GUIContent guiContentID = new GUIContent("ID", "A unique ID that can be used to identify the transition");
        private GUIContent guiMethod = new GUIContent("MethodName", "Functions returns bool included GameObject that has StateMachineBehaviour are dropped down");
        private GUIContent guiIsNegative = new GUIContent("IsNegative", "Whether to invert the result");

        private void OnEnable()
        {
            var inspectorHelper = target as TransitionInspectorHelper;

            var stateMachine = inspectorHelper.StateMachine;

            if (stateMachine != null)
            {
                serializedStateMachineObject = new SerializedObject(stateMachine);

                var stateArray = serializedStateMachineObject.FindProperty("graph").FindPropertyRelative("transitions");

                for (int i = 0; i < stateArray.arraySize; i++)
                {
                    var nameProperty = stateArray.GetArrayElementAtIndex(i).FindPropertyRelative("id");

                    if (nameProperty.stringValue == inspectorHelper.TransitionID)
                    {
                        var elementProperty = stateArray.GetArrayElementAtIndex(i);
                        
                        conditionMethodNameProperty = elementProperty.FindPropertyRelative("conditionMethodName");
                        isNegativeProperty = elementProperty.FindPropertyRelative("isNegative");
                        
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

            var inspectorHelper = target as TransitionInspectorHelper;
            var stateMachine = serializedStateMachineObject.targetObject as StateMachineBehaviour;
            var graph = stateMachine.GetStateMachineGraph();

            if (inspectorHelper == null || graph == null)
            {
                return;
            }

            if (graph.HasTransition(inspectorHelper.TransitionID))
            {
                serializedStateMachineObject.Update();

                EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
                {
                    // ConditionMethod
                    EditorGUILayout.LabelField(guiMethod);
                    CollectMethods();
                    var selected = conditionMethodNames.Select((name, index) => new {name, index})
                        .FirstOrDefault(itr => itr.name == conditionMethodNameProperty.stringValue);
                    int selectedIndex = selected?.index ?? 0;
                    conditionMethodNameProperty.stringValue =
                        conditionMethodNames[EditorGUILayout.Popup(selectedIndex, conditionMethodNames.ToArray())];
                    
                    // IsNegative
                    EditorGUILayout.Space();
                    isNegativeProperty.boolValue = EditorGUILayout.Toggle(guiIsNegative, isNegativeProperty.boolValue);
                }
                EditorGUI.EndDisabledGroup();
                
                serializedStateMachineObject.ApplyModifiedProperties();
            }
        }

        protected override void OnHeaderGUI()
        {
            if (serializedStateMachineObject == null || serializedStateMachineObject.targetObject == null)
            {
                return;
            }

            var inspectorHelper = target as TransitionInspectorHelper;
            var stateMachine = serializedStateMachineObject.targetObject as StateMachineBehaviour;
            var graph = stateMachine.GetStateMachineGraph();

            if (inspectorHelper == null || graph == null)
            {
                return;
            }

            var transition = inspectorHelper.Transition;

            if (transition != null)
            {
                EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying || PrefabUtility.IsPartOfAnyPrefab(stateMachine));
                {
                    EditorGUILayout.Space();
                    DrawRenameField(stateMachine, transition);
                    EditorGUILayout.Space();
                    DrawHorizontalDivider();
                    EditorGUILayout.Space();
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        private void DrawHorizontalDivider()
        {
            var rect = EditorGUILayout.BeginHorizontal();
            {
                Handles.color = Color.black;
                Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));

            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawRenameField(StateMachineBehaviour stateMachine, GraphTransition transition)
        {
            string description = transition.Description;
            
            EditorGUI.BeginChangeCheck();
            {
                EditorGUIUtility.labelWidth = 40;
                description = EditorGUILayout.DelayedTextField(guiContentDesc, description);
                EditorGUIUtility.labelWidth = 0;
            }
            if (EditorGUI.EndChangeCheck())
            {
                transition.Description = description;
            }
            
            string id = transition.ID;
            
            EditorGUI.BeginChangeCheck();
            {
                EditorGUIUtility.labelWidth = 40;
                id = EditorGUILayout.DelayedTextField(guiContentID, id);
                EditorGUIUtility.labelWidth = 0;
            }
            if (EditorGUI.EndChangeCheck())
            {
                if (stateMachine.TryRenameTransition(transition, id))
                {
                    TransitionInspectorHelper.Instance.Inspect(stateMachine, transition);
                }
            }
        }

        void CollectMethods()
        {
            conditionMethodNames.Clear();
            conditionMethodNames.Add("None");
            
            var inspectorHelper = target as TransitionInspectorHelper;
            var stateMachine = serializedStateMachineObject.targetObject as StateMachineBehaviour;
            
            if (inspectorHelper == null || stateMachine == null)
            {
                return;
            }

            MonoBehaviour[] components = stateMachine.gameObject.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                var methodNames = component.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(itr => itr.DeclaringType == component.GetType())
                    .Where(itr => itr.GetParameters().Length == 0)
                    .Where(itr => itr.ReturnType == typeof(bool))
                    .Select(itr => $"{component.GetType().FullName}/{itr.Name}")
                    .ToList();
                
                conditionMethodNames.AddRange(methodNames);
            }
        }
    }
}