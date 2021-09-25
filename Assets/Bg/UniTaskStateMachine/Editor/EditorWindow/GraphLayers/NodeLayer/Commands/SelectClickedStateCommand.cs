using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor.Commands
{
    public class SelectClickedStateCommand : ICommand
    {
        private readonly Context context;
        private readonly GraphNode node;

        public SelectClickedStateCommand(Context context, GraphNode node)
        {
            this.context = context;
            this.node = node;
        }

        public void Execute()
        {
            if (!context.SelectedNodes.Contains(node))
            {
                context.SelectedNodes.Clear();
                context.SelectedNodes.Add(node);
            }
            
            Event.current.Use();
        }
    }
}