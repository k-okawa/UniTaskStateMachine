using Bg.UniTaskStateMachine.Editor.Commands;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class TransitionContextMenu : IContextMenu
    {
        private readonly StateMachineBehaviour stateMachine;
        private readonly GraphTransition transition;

        public TransitionContextMenu(StateMachineBehaviour stateMachine, GraphTransition transition)
        {
            this.stateMachine = stateMachine;
            this.transition = transition;
        }

        public void Show()
        {
            var genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("DeleteTransition"), false, () =>
            {
                stateMachine.DeleteTransition(transition);
            });
            
            genericMenu.ShowAsContext();
        }
    }
}