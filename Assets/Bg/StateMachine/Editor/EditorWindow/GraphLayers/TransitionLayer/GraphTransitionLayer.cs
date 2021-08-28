using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bg.StateMachine.Editor
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

        private void DrawTransition(GraphTransition transition)
        {
            if (TryGetTransitionNodes(transition, out GraphNode originNode, out GraphNode targetNode))
            {
                Color color = Color.white;

                Rect startRect = GetTransformedRect(originNode.Rect);

                Rect endRect = GetTransformedRect(targetNode.Rect);

                DrawTransition(startRect.center, endRect, color);
            }
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

        protected override void OnLeftMouseButtonEvent(Vector2 mousePos)
        {
            
        }

        protected override void OnRightMouseButtonEvent(Vector2 mousePos)
        {
            if (Context.TransitionPreview != null)
            {
                switch (Event.current.type)
                {
                    case EventType.MouseUp:
                        Context.TransitionPreview = null;
                        Event.current.Use();
                        break;
                }

                return;
            }
        }

        protected override void OnMouseMoveEvent(Vector2 mousePos)
        {
            if (Context.TransitionPreview != null)
            {
                GUI.changed = true;
            }
        }
    }
}