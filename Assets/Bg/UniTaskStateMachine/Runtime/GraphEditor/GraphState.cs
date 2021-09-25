using UnityEngine;

namespace Bg.UniTaskStateMachine
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