using UnityEditor;
using UnityEngine;

namespace Bg.StateMachine.Editor.ContextMenus
{
    public class StateContextMenu : IContextMenu
    {
        private readonly Context context;
        private readonly GraphState state;

        public StateContextMenu(Context context, GraphState state)
        {
            this.context = context;
            this.state = state;
        }

        public void Show()
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Make Transition"), false, () =>
            {
                this.context.TransitionPreview = this.state;
            });
            genericMenu.ShowAsContext();
        }
    }
}