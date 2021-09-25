using System.Linq;
using UnityEditor;

namespace Bg.UniTaskStateMachine.Editor
{
    public class DeleteNodeCommand : ICommand
    {
        private readonly StateMachineBehaviour stateMachine;
        private readonly GraphNode node;

        public DeleteNodeCommand(StateMachineBehaviour stateMachine, GraphNode node)
        {
            this.stateMachine = stateMachine;
            this.node = node;
        }

        public void Execute()
        {
            Undo.RegisterCompleteObjectUndo(stateMachine, "Remove node");

            var graph = stateMachine.Graph;

            var transitions = graph.Transitions
                .Where(itr => itr.OriginStateID == node.ID || itr.TargetStateID == node.ID).ToList();

            foreach (var transition in transitions)
            {
                graph.Transitions.Remove(transition);
            }

            graph.TryRemoveNode(node);

            if (graph.EntryStateId == node.ID)
            {
                graph.EntryStateId = string.Empty;
            }
        }
    }
}