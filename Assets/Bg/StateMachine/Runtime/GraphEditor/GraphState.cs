using UnityEngine;

namespace Bg.StateMachine
{
    [System.Serializable]
    public class GraphState : GraphNode
    {
        [SerializeField] 
        private BaseStateComponent stateComponent;

        public BaseStateComponent StateComponent
        {
            get => stateComponent;
        }
    }
}