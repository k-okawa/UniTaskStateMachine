using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class AddStateCommand : ICommand
    {
        private readonly StateMachineBehaviour stateMachine;
        private readonly Vector2 position;

        public AddStateCommand(StateMachineBehaviour stateMachine, Vector2 position)
        {
            this.stateMachine = stateMachine;
            this.position = position;
        }
        
        public void Execute()
        {
            var graph = stateMachine.Graph;
            
            var rect = new Rect(0, 0, 120, 60);
            rect.x = this.position.x - rect.width / 2;
            rect.y = this.position.y - rect.height / 2;

            var node = new GraphState
            {
                Rect = rect,
                ID = graph.GetUniqueStateName()
            };
            
            Undo.RegisterCompleteObjectUndo(this.stateMachine, "Added node");

            if (graph.TryAddNode(node))
            {
                if (graph.Nodes.Count == 1)
                {
                    graph.EntryStateId = node.ID;
                }
            }
        }
    }
}