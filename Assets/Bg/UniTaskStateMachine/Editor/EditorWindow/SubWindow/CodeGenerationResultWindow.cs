using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor.SubWindow
{
    public class CodeGenerationResultWindow : EditorWindow
    {
        private Vector2 _scroll;
        private string text;

        public static void Open(string title, string code)
        {
            var window = GetWindow<CodeGenerationResultWindow>(title);
            window.text = code;
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("GenerationResult");
            if (GUILayout.Button("Copy"))
            {
                EditorGUIUtility.systemCopyBuffer = text;
            }
            EditorGUILayout.EndHorizontal();

            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            text = EditorGUILayout.TextArea(text, EditorStyles.textArea);
            EditorGUILayout.EndScrollView();
            
            
            EditorGUILayout.EndVertical();
        }
    }
}