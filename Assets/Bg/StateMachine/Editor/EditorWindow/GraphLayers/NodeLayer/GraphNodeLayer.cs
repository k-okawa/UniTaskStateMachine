using Bg.StateMachine.Editor.Commands;
using UnityEngine;
using UnityEditor;

namespace Bg.StateMachine.Editor
{
    public class GraphNodeLayer : GraphLayer
    {
        private readonly StateStyles stateStyles;

        private bool isDragging = false;

        public GraphNodeLayer(EditorWindow editorWindow) : base(editorWindow)
        {
            this.stateStyles = new StateStyles();
        }

        public override void Draw(Rect rect)
        {
            base.Draw(rect);
            DrawNodes(rect);
        }

        private void DrawNodes(Rect rect)
        {
            stateStyles.ApplyZoomFactor(Context.ZoomFactor);

            var graph = Context.Graph;

            foreach (var node in graph.Nodes)
            {
                Rect nodeRect = GetTransformedRect(node.Rect);

                if (rect.Overlaps(nodeRect))
                {
                    DrawNode(node, nodeRect);
                }
            }
        }

        private void DrawNode(GraphNode node, Rect rect)
        {
            string nodeName = node.ID;

            var style = StateStyles.Style.Normal;

            bool isSelected = Context.SelectedNodes.Contains(node);
            if (node is GraphState state)
            {
                style = isSelected ? StateStyles.Style.NormalOn : StateStyles.Style.Normal;
            }
            
            GUI.Box(rect, nodeName, stateStyles.Get(style));
        }

        protected override void OnLeftMouseButtonEvent(Vector2 mousePos)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                {
                    isDragging = false;
                    Context.SelectedNodes.Clear();

                    var node = Context.Graph.GetClickedNode(this, mousePos);

                    if (node != null)
                    {
                        if (Context.TransitionPreview != null && Context.TransitionPreview != node)
                        {
                            if (node is GraphState state)
                            {
                                Context.StateMachine.AddTransition(Context.TransitionPreview, state);
                                Context.TransitionPreview = null;
                            }
                        }
                        
                        if (!Context.SelectedNodes.Contains(node))
                        {
                            Context.SelectedNodes.Add(node);
                        }
                    }
                    
                    Event.current.Use();
                    
                    break;
                }

                case EventType.MouseDrag:
                {
                    if (!isDragging)
                    {
                        isDragging = true;
                    }
                    else
                    {
                        var graphRect = EditorWindow.Rect;
                        graphRect.yMin += EditorStyles.toolbar.fixedHeight;
                        if (!graphRect.Contains(mousePos))
                        {
                            isDragging = false;
                            break;
                        }
                        
                        if (Context.SelectedNodes.Count > 0)
                        {
                            Event.current.Use();
                            GUI.changed = true;
                        }
                    
                        foreach (var node in Context.SelectedNodes)
                        {
                            Rect rect = node.Rect;
                    
                            rect.x += Event.current.delta.x / this.Context.ZoomFactor;
                            rect.y += Event.current.delta.y / this.Context.ZoomFactor;
                    
                            node.Rect = rect;
                        }
                    }

                    break;
                }

                case EventType.MouseUp:
                {
                    if (isDragging)
                    {
                        isDragging = false;
                    }
                    break;
                }
            }
        }

        protected override void OnRightMouseButtonEvent(Vector2 mousePos)
        {
            var node = Context.Graph.GetClickedNode(this, mousePos);

            if (node != null)
            {
                ICommand command = null;

                switch (Event.current.type)
                {
                    case EventType.MouseDown:
                        command = new SelectClickedStateCommand(Context, node);
                        break;
                    case EventType.MouseUp:
                        isDragging = false;
                        command = new ShowContextMenuCommand(Context, node);
                        break;
                }
                command?.Execute();
            }
        }

        protected override void OnKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.Delete)
            {
                Context.GraphSelection.Delete();
                EditorWindow.Repaint();
            }
        }
    }
}