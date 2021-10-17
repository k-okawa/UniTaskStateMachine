using Bg.UniTaskStateMachine.Editor.Commands;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class GraphContextMenu : IContextMenu
    {
        private readonly EditorWindow view;
        private readonly Vector2 mousePosition;

        public GraphContextMenu(EditorWindow view, Vector2 mousePosition)
        {
            this.view = view;
            this.mousePosition = mousePosition;
        }
        
        public void Show()
        {
            Context context = view.Context;

            Vector2 nodePosition = GetNodePosition(mousePosition - context.DragOffset);
            
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Create State"), false, () =>
            {
                CreateState(nodePosition);
            });
            genericMenu.ShowAsContext();
        }

        private void CreateState(Vector2 position)
        {
            view.Context.StateMachine.AddState(position);
        }
        
        private Vector2 GetNodePosition(Vector2 position)
        {
            EditorWindow window = this.view;
            
            Vector2 distance = (position + window.Context.DragOffset - new Vector2(window.Rect.width, window.Rect.height) / 2) * (1 - window.Context.ZoomFactor);

            return new Vector2()
            {
                x = position.x + 2 * distance.x,
                y = position.y + 2 * distance.y
            };
        }
    }
}