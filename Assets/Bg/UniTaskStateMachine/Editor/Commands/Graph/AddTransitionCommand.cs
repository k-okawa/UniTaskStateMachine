using System.Linq;
using UnityEditor;

namespace Bg.UniTaskStateMachine.Editor
{
    public class AddTransitionCommand : ICommand
    {
        private readonly StateMachineBehaviour stateMachine;
        private readonly GraphNode origin;
        private readonly GraphState target;

        public AddTransitionCommand(StateMachineBehaviour stateMachine, GraphNode origin, GraphState target)
        {
            this.stateMachine = stateMachine;
            this.origin = origin;
            this.target = target;
        }

        public void Execute()
        {
            var graph = stateMachine.Graph;

            if (graph.Transitions.Any(itr => itr.OriginStateID == origin.ID && itr.TargetStateID == target.ID))
            {
                return;
            }
            
            var transition = new GraphTransition
            {
                ID = graph.GetUniqueTransitionId(),
                OriginStateID = origin.ID,
                TargetStateID = target.ID
            };
            
            Undo.RegisterCompleteObjectUndo(stateMachine, "Add transition");

            graph.TryAddTransition(transition);
        }
    }
}