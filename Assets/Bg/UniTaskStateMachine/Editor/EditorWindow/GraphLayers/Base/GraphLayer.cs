using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class GraphLayer
    {
        public enum MouseButton { Left = 0, Right = 1, Middle = 2 }
        
        public EditorWindow EditorWindow { get; }
        
        protected Context Context => this.EditorWindow.Context;
        
        private Matrix4x4 transformMatrix = Matrix4x4.identity;

        public GraphLayer(EditorWindow editorWindow)
        {
            this.EditorWindow = editorWindow;
        }

        private void UpdateTransformationMatrix()
        {
            var window = this.EditorWindow;

            var centerMat = Matrix4x4.Translate(-window.Rect.size / 2);
            var translationMat = Matrix4x4.Translate(window.Context.DragOffset);
            var scaleMat = Matrix4x4.Scale(Vector3.one * window.Context.ZoomFactor);

            this.transformMatrix = centerMat.inverse * scaleMat * translationMat * centerMat;
        }

        public virtual void Draw(Rect rect)
        {
            UpdateTransformationMatrix();
        }

        public void ProcessEvents(Vector2 mousePos)
        {
            UpdateTransformationMatrix();

            Event currentEvent = Event.current;
            
            //Mouse events
            if (currentEvent.isMouse || currentEvent.rawType == EventType.MouseUp)
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseMove:
                        OnMouseMoveEvent(mousePos);
                        break;
                }

                switch ((MouseButton)currentEvent.button)
                {
                    case MouseButton.Left:
                        OnLeftMouseButtonEvent(mousePos);
                        break;
                    case MouseButton.Right:
                        OnRightMouseButtonEvent(mousePos);
                        break;
                    case MouseButton.Middle:
                        OnMiddleMouseButtonEvent(mousePos);
                        break;
                    default:
                        break;
                }
            }
            
            //ScrollWheel events
            if (currentEvent.isScrollWheel)
            {
                if (currentEvent.type == EventType.ScrollWheel)
                {
                    OnScrollWheelMoved();
                }
            }

            if (currentEvent.isKey)
            {
                switch (currentEvent.type)
                {
                    case EventType.KeyDown:
                        OnKeyDown(currentEvent.keyCode);
                        break;
                    case EventType.KeyUp:
                        OnKeyUp(currentEvent.keyCode);
                        break;
                }
            }
        }
        
        // events
        protected virtual void OnMouseMoveEvent(Vector2 mousePos) { }

        protected virtual void OnLeftMouseButtonEvent(Vector2 mousePos) { }
        protected virtual void OnRightMouseButtonEvent(Vector2 mousePos) { }
        protected virtual void OnMiddleMouseButtonEvent(Vector2 mousePos) { }

        protected virtual void OnScrollWheelMoved() { }
        
        protected virtual void OnKeyDown(KeyCode keyCode) { }
        protected virtual void OnKeyUp(KeyCode keyCode) { }

        /// <summary>
        /// Computes and returns a transformed version of a given rect by applying the current offset and the zoom factor.
        /// </summary>
        public Rect GetTransformedRect(Rect rect, bool isAdjustGrid = true)
        {
            // adjust grid
            if (isAdjustGrid)
            {
                rect.x -= (rect.x % GraphEnvironment.GraphGridSpace) - (int)(GraphEnvironment.GraphGridSpace / 2);
                rect.y -= (rect.y % GraphEnvironment.GraphGridSpace) - (int)(GraphEnvironment.GraphGridSpace / 2);
            }
            
            Rect result = new Rect
            {
                position = transformMatrix.MultiplyPoint(rect.position),
                size = transformMatrix.MultiplyVector(rect.size)
            };

            return result;
        }
    }
}