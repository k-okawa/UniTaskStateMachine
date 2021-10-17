using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class GraphView
    {
        private Context Context { get; }
        
        private List<GraphLayer> Layers { get; } = new List<GraphLayer>();

        public GraphView(EditorWindow editorWindow)
        {
            this.Context = editorWindow.Context;
            
            this.Layers.Add(new GraphBackgroundLayer(editorWindow));
            this.Layers.Add(new GraphTransitionLayer(editorWindow));
            this.Layers.Add(new GraphNodeLayer(editorWindow));
        }

        public void Repaint(Rect rect)
        {
            if (!Context.IsStateMachineLoaded)
            {
                return;
            }
            
            EditorGUI.DrawRect(rect, GraphEnvironment.BackgroundColor);

            for (int i = 0; i < this.Layers.Count; i++)
            {
                this.Layers[i].Draw(rect);
            }
        }

        public void ProcessEvents(Rect rect)
        {
            if (!Context.IsStateMachineLoaded)
            {
                return;
            }
            
            for (int i = this.Layers.Count - 1; i >= 0; i--)
            {
                this.Layers[i].ProcessEvents(Event.current.mousePosition);
            }
        }
    }
}