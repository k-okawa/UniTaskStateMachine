﻿using UnityEngine;

namespace Bg.StateMachine.Editor.Commands
{
    public static class GraphCommands
    {
        public static void AddState(this StateMachineBehaviour stateMachine, Vector2 position)
        {
            ICommand command = new AddStateCommand(stateMachine, position);
            command.Execute();
        }

        public static void AddTransition(this StateMachineBehaviour stateMachine, GraphNode origin, GraphState target)
        {
            ICommand command = new AddTransitionCommand(stateMachine, origin, target);
            command.Execute();
        }

        public static void DeleteNode(this StateMachineBehaviour stateMachine, GraphNode node)
        {
            ICommand command = new DeleteNodeCommand(stateMachine, node);
            command.Execute();
        }
    }
}