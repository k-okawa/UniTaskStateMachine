using System.Collections.Generic;
using Bg.UniTaskStateMachine.Editor.Commands;
using UnityEngine;
using UnityEditor;

namespace Bg.UniTaskStateMachine.Editor
{
    public class GraphTransitionLayer : GraphLayer
    {
        private Dictionary<string, GraphNode> nodeDic = new Dictionary<string, GraphNode>();
        
        public GraphTransitionLayer(EditorWindow window) : base(window)
        {
            
        }

        public override void Draw(Rect rect)
        {
            base.Draw(rect);
            
            UpdateNodeDictionary();

            DrawTransitions();
            DrawTransitionPreview();
        }

        private void UpdateNodeDictionary()
        {
            nodeDic.Clear();

            foreach (var node in Context.Graph.Nodes)
            {
                nodeDic.Add(node.ID, node);
            }
        }

        private bool TryGetTransitionNodes(GraphTransition transition, out GraphNode origin, out GraphNode target)
        {
            if (Context.Graph.TryGetNode(transition.OriginStateID, out origin) &&
                Context.Graph.TryGetNode(transition.TargetStateID, out target))
            {
                return true;
            }

            origin = null;
            target = null;

            return false;
        }

        private void DrawTransitions()
        {
            var graph = Context.Graph;

            foreach (var transition in graph.Transitions)
            {
                DrawTransition(transition);
            }
        }
        
        private void DrawTransitionPreview()
        {
            if (Context.TransitionPreview != null)
            {
                DrawTransition(GetTransformedRect(this.Context.TransitionPreview.Rect).center, new Rect(Event.current.mousePosition, Vector2.one), Color.white);
            }
        }
        
        private Vector2 GetTransitionOffset(GraphNode originNode, Rect originRect, Rect targetRect)
        {
            Vector2 distance = originRect.center - targetRect.center;

            Vector2 offset = Vector2.zero;

            if (Mathf.Abs(distance.y) > Mathf.Abs(distance.x))
            {
                offset.x = Mathf.Sign(distance.y) * 10.0f;
            }
            else
            {
                offset.y = Mathf.Sign(distance.x) * 10.0f;
            }

            return offset * this.Context.ZoomFactor;
        }

        private void DrawTransition(GraphTransition transition)
        {
            if (TryGetTransitionNodes(transition, out GraphNode originNode, out GraphNode targetNode))
            {
                Color color = GetTransitionColor(transition);

                Rect startRect = GetTransformedRect(originNode.Rect);

                Rect endRect = GetTransformedRect(targetNode.Rect);

                Vector2 offset = GetTransitionOffset(originNode, startRect, endRect);
                startRect.center += offset;
                endRect.center += offset;

                DrawTransition(startRect.center, endRect, color);
            }
        }

        private Color GetTransitionColor(GraphTransition transition)
        {
            if (Selection.activeObject == TransitionInspectorHelper.Instance && TransitionInspectorHelper.Instance.TransitionID == transition.ID)
            {
                return GraphEnvironment.SelectionColor;
            }

            return Color.white;
        }

        private void DrawTransition(Vector2 startPos, Rect end, Color color)
        {
            Line transitionLine = new Line(startPos, end.center);
            
            float angle = Vector2.SignedAngle(Vector2.right, transitionLine.Direction) + 90.0f;
            
            Vector3[] triangle = {
                new Vector2(-1, 0.5f) * (8 * this.Context.ZoomFactor),
                new Vector2(1, 0.5f) * (8 * this.Context.ZoomFactor),
                new Vector2(0, -0.5f) * (18 * this.Context.ZoomFactor)
            };
            
            for (int i = 0; i < triangle.Length; i++)
            {
                triangle[i] = Quaternion.Euler(0, 0, angle) * triangle[i];

                Vector2 pos = transitionLine.Lerp(0.45f);

                triangle[i] += (Vector3)(pos);
            }
            
            if (transitionLine.Intersects(EditorWindow.Rect))
            {
                //Begin drawing
                Handles.BeginGUI();

                Handles.color = color;

                //Draw line
                Handles.DrawAAPolyLine(6.0f * this.Context.ZoomFactor, startPos, end.center);

                //Draw triangle
                Handles.DrawAAConvexPolygon(triangle);

                //End drawing
                Handles.EndGUI();
            }
        }

        public GraphTransition GetClickedTransition(Vector2 mousePos)
        {
            float maxDistance = 5f * Context.ZoomFactor;

            var graph = Context.Graph;

            foreach (var transition in graph.Transitions)
            {
                if (TryGetTransitionNodes(transition, out GraphNode originNode, out GraphNode targetNode))
                {
                    Rect originRect = GetTransformedRect(originNode.Rect);
                    Rect targetRect = GetTransformedRect(targetNode.Rect);

                    Vector2 a = originRect.center + GetTransitionOffset(originNode, originRect, targetRect);
                    Vector2 c = targetRect.center + GetTransitionOffset(originNode, originRect, targetRect);
                    
                    Line transitionLine = new Line(a, c);
                    
                    Rect rect = new Rect()
                    {
                        x = Mathf.Min(a.x, c.x) - maxDistance,
                        y = Mathf.Min(a.y, c.y) - maxDistance,
                        width = Mathf.Max(a.x, c.x) - Mathf.Min(a.x, c.x) + 2 * maxDistance,
                        height = Mathf.Max(a.y, c.y) - Mathf.Min(a.y, c.y) + 2 * maxDistance
                    };

                    if (rect.Contains(mousePos))
                    {
                        if (transitionLine.GetMinDistance(mousePos) < maxDistance)
                        {
                            return transition;
                        }
                    }
                }
            }

            return null;
        }

        protected override void OnLeftMouseButtonEvent(Vector2 mousePos)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    GraphTransition transition = GetClickedTransition(mousePos);
                    if (transition != null)
                    {
                        Context.SelectedNodes.Clear();
                        TransitionInspectorHelper.Instance.Inspect(Context.StateMachine, transition);
                        Event.current.Use();
                    }
                    break;
            }
        }

        protected override void OnRightMouseButtonEvent(Vector2 mousePos)
        {
            if (EditorApplication.isPlaying || Context.IsPrefabAsset)
            {
                return;
            }
            
            GraphTransition transition = GetClickedTransition(mousePos);

            if (transition == null)
            {
                return;
            }

            switch (Event.current.type)
            {
                case EventType.MouseUp:
                    Event.current.Use();
                    IContextMenu contextMenu = new TransitionContextMenu(Context.StateMachine, transition);
                    contextMenu.Show();
                    break;
            }
        }

        protected override void OnMouseMoveEvent(Vector2 mousePos)
        {
            if (Context.TransitionPreview != null)
            {
                GUI.changed = true;
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
                if (Context.SelectedNodes.Count == 0)
                {
                    if (Selection.activeObject == TransitionInspectorHelper.Instance)
                    {
                        var transition = TransitionInspectorHelper.Instance.Transition;

                        if (Context.Graph.Transitions.Contains(transition))
                        {
                            Context.StateMachine.DeleteTransition(transition);
                            Selection.activeObject = null;
                        }
                    }
                }
            }
        }
    }
}