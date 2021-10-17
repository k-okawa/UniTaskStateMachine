using UnityEditor;

namespace Bg.UniTaskStateMachine.Editor
{
    public class DeleteTransitionCommand : ICommand
    {
        private readonly StateMachineBehaviour stateMachine;
        private readonly GraphTransition transition;

        public DeleteTransitionCommand(StateMachineBehaviour stateMachine, GraphTransition transition)
        {
            this.stateMachine = stateMachine;
            this.transition = transition;
        }

        public void Execute()
        {
            var graph = stateMachine.Graph;

            Undo.RegisterCompleteObjectUndo(this.stateMachine, "Remove transition");

            graph.TryRemoveTransition(this.transition);
        }
    }
}