using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    [System.Serializable]
    public class GraphBackgroundLayer : GraphLayer
    {
        public GraphBackgroundLayer(EditorWindow view) : base(view) {}

        protected override void OnLeftMouseButtonEvent(Vector2 mousePos)
        {
            if (EditorApplication.isPlaying || Context.IsPrefabAsset)
            {
                if (Event.current.type == EventType.MouseDown)
                {
                    Selection.activeObject = null;
                    Context.SelectedNodes.Clear();
                }

                return;
            }

            if (Event.current.type == EventType.MouseDown)
            {
                Selection.activeObject = null;
                Context.SelectedNodes.Clear();
                Event.current.Use();
                Context.TransitionPreview = null;
                Context.SelectionRect.Position = Event.current.mousePosition;
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                if (Context.SelectionRect.IsActive)
                {
                    Context.SelectionRect.Drag(Event.current.mousePosition);
                    Event.current.Use();
                }
            }
            else if (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseUp)
            {
                var graph = Context.Graph;

                foreach (var node in graph.Nodes)
                {
                    Rect transformedRect = GetTransformedRect(node.Rect);

                    if (Context.SelectionRect.Contains(new Vector2(transformedRect.xMax, transformedRect.yMax)))
                    {
                        Context.SelectedNodes.Add(node);
                    }
                }

                Context.SelectionRect.Reset();
            }
        }

        protected override void OnRightMouseButtonEvent(Vector2 mousePos)
        {
            if (EditorApplication.isPlaying || Context.IsPrefabAsset)
            {
                return;
            }
            
            if (Event.current.type == EventType.MouseUp)
            {
                IContextMenu contextMenu = new GraphContextMenu(EditorWindow, mousePos);
                contextMenu.Show();
                Event.current.Use();
            }
        }

        protected override void OnMiddleMouseButtonEvent(Vector2 mousePos)
        {
            switch (Event.current.type)
            {
                //Adjust the offset of the views when the user holds the mouse wheel and drags
                case EventType.MouseDrag:

                    //Adjust drag offset
                    this.Context.DragOffset += Event.current.delta / this.Context.ZoomFactor;

                    //Use current event and update GUI
                    Event.current.Use();

                    break;
            }
        }

        public override void Draw(Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                DrawGrid(GraphEnvironment.GraphGridSpace, GraphEnvironment.OuterGridColor, rect.width, rect.height);
                DrawGrid(GraphEnvironment.GraphGridSpace * 10f, GraphEnvironment.InnerGridColor, rect.width, rect.height);
            } 
        }
        
        private void DrawGrid(float gridSpacing, Color gridColor, float width, float height)
        {
            Vector2 vanishingPoint = new Vector2(width, height) / 2;

            int widthDivs = Mathf.CeilToInt(width / this.Context.ZoomFactor / gridSpacing);
            int heightDivs = Mathf.CeilToInt(height / this.Context.ZoomFactor / gridSpacing);

            Vector2 newOffset = new Vector3(this.Context.DragOffset.x % gridSpacing, this.Context.DragOffset.y % gridSpacing);

            GL.PushMatrix();
            GL.LoadPixelMatrix();
            GL.Begin(GL.LINES);
            GL.Color(gridColor);

            for (int i = -widthDivs; i < widthDivs / this.Context.ZoomFactor; i++)
            {
                float distance = (gridSpacing * i + newOffset.x - vanishingPoint.x) * (1 - this.Context.ZoomFactor);

                float x = gridSpacing * i + newOffset.x - distance;

                Vector2 start = new Vector2(x, -gridSpacing);
                Vector2 end = new Vector2(x, height);

                GL.Vertex(start);
                GL.Vertex(end);
            }

            for (int j = -heightDivs; j < heightDivs / this.Context.ZoomFactor; j++)
            {
                float distance = (gridSpacing * j + newOffset.y - vanishingPoint.y) * (1 - this.Context.ZoomFactor);

                float y = gridSpacing * j + newOffset.y - distance;

                Vector2 start = new Vector2(-gridSpacing + 1, y);
                Vector2 end = new Vector2(width, y);

                GL.Vertex(start);
                GL.Vertex(end);
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}