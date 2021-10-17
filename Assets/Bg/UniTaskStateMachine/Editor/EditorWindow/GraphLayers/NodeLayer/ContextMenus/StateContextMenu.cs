using Bg.UniTaskStateMachine.Editor.Commands;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor.ContextMenus
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
                context.TransitionPreview = this.state;
            });
            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Set as Entry"), false, () =>
            {
                Undo.RegisterCompleteObjectUndo(this.context.StateMachine, "Set entry state");
                this.context.Graph.EntryStateId = this.state.ID;
            });
            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Delete"), false, () =>
            {
                context.SelectedNodes.Clear();
                context.StateMachine.DeleteNode(this.state);
            });
            genericMenu.ShowAsContext();
        }
    }
}