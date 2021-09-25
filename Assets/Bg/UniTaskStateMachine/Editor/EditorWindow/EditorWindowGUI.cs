using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class EditorWindowGUI
    {
        [System.NonSerialized] private readonly EditorWindow editorWindow;
        
        private Toolbar Toolbar { get; }
        
        private GraphView Graph { get; }

        public EditorWindowGUI(EditorWindow editorWindow)
        {
            this.editorWindow = editorWindow;

            this.Toolbar = new Toolbar(this.editorWindow);
            
            this.Graph = new GraphView(this.editorWindow);
        }

        public void OnGUI()
        {
            var rect = editorWindow.Rect;

            if (Event.current.type == EventType.Repaint)
            {
                this.Graph.Repaint(rect);
            }
            
            this.Toolbar.OnGUI(editorWindow.Rect);
            
            if (Event.current.isMouse || 
                Event.current.isKey || 
                Event.current.isScrollWheel || 
                Event.current.rawType == EventType.MouseUp)
            {
                this.Graph.ProcessEvents(editorWindow.Rect);
            }

            if (GUI.changed)
            {
                editorWindow.Repaint();
            }
        }
    }
}