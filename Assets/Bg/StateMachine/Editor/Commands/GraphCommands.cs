using UnityEngine;

namespace Bg.StateMachine.Editor.Commands
{
    public static class GraphCommands
    {
        public static void AddState(this StateMachineBehaviour stateMachine, Vector2 position)
        {
            ICommand command = new AddStateCommand(stateMachine, position);
            command.Execute();
        }
    }
}