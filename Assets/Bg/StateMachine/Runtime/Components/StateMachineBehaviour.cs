using UnityEngine;

namespace Bg.StateMachine
{
    [DefaultExecutionOrder(-1)]
    [DisallowMultipleComponent]
    public class StateMachineBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Graph graph = new Graph();

        public Graph Graph => graph;
    }
}