using System;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class EditorWindow : UnityEditor.EditorWindow
    {
        public Context Context { get; private set; } = new Context();
        
        [System.NonSerialized] private EditorWindowGUI editorWindowGUI;

        private bool IsEnabled { get; set; } = false;
        
        public Rect Rect
        {
            get { return new Rect(0, 0, this.position.width, this.position.height); }
        }
        
        private void OnEnable()
        {
            wantsMouseMove = true;
            this.editorWindowGUI = new EditorWindowGUI(this);
            Context.Reload();
            IsEnabled = true;
        }

        private void Update()
        {
            
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnHierarchyChange()
        {
            Context.Reload();
        }

        private void OnSelectionChange()
        {
            if (IsEnabled)
            {
                Context.UpdateSelection();
            }
        }

        private void OnFocus()
        {
            if (IsEnabled)
            {
                Context.Reload();
            }
        }

        private void OnGUI()
        {
            this.editorWindowGUI.OnGUI();
        }
    }
}