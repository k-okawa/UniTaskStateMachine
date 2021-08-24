using UnityEditor;
using UnityEngine;

namespace Bg.StateMachine.Editor
{
    public class EditorWindow : UnityEditor.EditorWindow
    {
        public Context Context { get; private set; } = new Context();
        
        [System.NonSerialized] private EditorWindowGUI editorWindowGUI;
        
        public Rect Rect
        {
            get { return new Rect(0, 0, this.position.width, this.position.height); }
        }
        
        private void OnEnable()
        {
            this.editorWindowGUI = new EditorWindowGUI(this);
        }

        private void OnGUI()
        {
            this.editorWindowGUI.OnGUI();
        }

        private void Update()
        {
            
        }

        private void OnInspectorUpdate()
        {
            
        }
    }
}