using Bg.UniTaskStateMachine.Editor.ContextMenus;
 using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor.Commands
{
    public class ShowContextMenuCommand : ICommand
    {
        private readonly Context context;
        private readonly GraphNode node;

        public ShowContextMenuCommand(Context context, GraphNode node)
        {
            this.context = context;
            this.node = node;
        }

        public void Execute()
        {
            IContextMenu contextMenu = null;
            
            this.context.SelectedNodes.Clear();

            if (node is GraphState state)
            {
                contextMenu = new StateContextMenu(context, state);
            }
            
            contextMenu?.Show();
            
            Event.current.Use();
        }
    }
}