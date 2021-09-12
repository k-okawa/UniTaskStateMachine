﻿using System.Collections.Generic;
using Bg.StateMachine.Editor.Commands;

namespace Bg.StateMachine.Editor
{
    public class DeleteSelectionCommand : ICommand
    {
        private readonly StateMachineBehaviour stateMachine;
        private readonly List<GraphNode> nodes;

        public DeleteSelectionCommand(StateMachineBehaviour stateMachine, List<GraphNode> nodes)
        {
            this.stateMachine = stateMachine;
            this.nodes = nodes;
        }

        public void Execute()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                stateMachine.DeleteNode(nodes[i]);
                nodes.RemoveAt(i);
                i--;
            }
        }
    }
}