using Bg.UniTaskStateMachine.Editor.Commands;
using UnityEngine;
using UnityEditor;

namespace Bg.UniTaskStateMachine.Editor
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
            Context.SelectionRect.Draw();
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
                if (Application.isPlaying && state.ID == Context.StateMachine.StateMachine.CurrentNode.Id)
                {
                    style = isSelected ? StateStyles.Style.OrangeOn : StateStyles.Style.Orange;
                }
                else if (state.ID == Context.Graph.EntryStateId)
                {
                    style = isSelected ? StateStyles.Style.GreenOn : StateStyles.Style.Green;
                }
                else
                {
                    style = isSelected ? StateStyles.Style.NormalOn : StateStyles.Style.Normal;
                }
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

                    var node = Context.Graph.GetClickedNode(this, mousePos);

                    if (node != null)
                    {
                        if (EditorApplication.isPlaying == false && Context.TransitionPreview != null && Context.TransitionPreview != node)
                        {
                            if (node is GraphState state)
                            {
                                Context.StateMachine.AddTransition(Context.TransitionPreview, state);
                                Context.TransitionPreview = null;
                            }
                        }
                        else if (EditorApplication.isPlaying == false && Event.current.control)
                        {
                            if (Context.SelectedNodes.Count > 0)
                            {
                                Selection.activeObject = null;
                            }

                            if (Context.SelectedNodes.Contains(node))
                            {
                                Context.SelectedNodes.Remove(node);
                            }
                            else
                            {
                                Context.SelectedNodes.Add(node);
                            }
                        }
                        else
                        {
                            if (Context.SelectedNodes.Count < 2 || !Context.SelectedNodes.Contains(node))
                            {
                                Context.SelectedNodes.Clear();
                                Context.SelectedNodes.Add(node);

                                if (node is GraphState state)
                                {
                                    StateInspectorHelper.Instance.Inspect(Context.StateMachine, Context.Graph, state);
                                }
                                else
                                {
                                    if (Selection.activeObject == StateInspectorHelper.Instance)
                                    {
                                        Selection.activeObject = null;
                                    }
                                }
                            }
                        }

                        Event.current.Use();
                    }

                    break;
                }

                case EventType.MouseDrag:
                {
                    if (Application.isPlaying || Context.IsPrefabAsset)
                    {
                        break;
                    }

                    if (!Event.current.control)
                    {
                        if (!isDragging)
                        {
                            Undo.RegisterCompleteObjectUndo(this.Context.StateMachine, "Dragged state");
                            isDragging = true;
                        }
                        else
                        {
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
            if (EditorApplication.isPlaying || Context.IsPrefabAsset)
            {
                return;
            }
            
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
            if (EditorApplication.isPlaying || Context.IsPrefabAsset)
            {
                return;
            }
            
            if (keyCode == KeyCode.Delete)
            {
                Context.GraphSelection.Delete();
                EditorWindow.Repaint();
            }
        }
    }
}