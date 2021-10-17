using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class ToolbarElement
    {
        protected EditorWindow EditorWindow { get; }
        protected Context Context { get; }
        
        protected float Width { get; set; }

        public ToolbarElement(EditorWindow window)
        {
            this.EditorWindow = window;
            this.Context = window.Context;
        }

        public virtual void OnGUI(Rect rect)
        {
            
        }
    }
}