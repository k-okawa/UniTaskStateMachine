using UnityEngine;

// MonoBehaviourを継承させるが、いったん継承を外している

namespace Bg.StateMachine
{
    [DefaultExecutionOrder(-1)]
    [DisallowMultipleComponent]
    public class StateMachineBehaviour
    {
        [SerializeField]
        private Graph graph = new Graph();

        public Graph Graph => graph;
    }
}