using System;
using System.Reflection;
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

        private void Start()
        {
            // TODO: this is test
            if (graph.TryGetTransition("TransitionStart", out GraphTransition transition))
            {
                string methodName = transition.ConditionMethodName;
                if (methodName != string.Empty && methodName != "None")
                {
                    var divided = methodName.Split('/');
                    string className = divided[0];
                    string method = divided[1];
                    var comp = GetComponent(className);
                    Type type = comp.GetType();
                    MethodInfo methodInfo = type.GetMethod(method);
                    bool isMatch = !(methodInfo is null) && (bool)methodInfo.Invoke(comp, null);
                    if (isMatch)
                    {
                        Debug.Log("Match Condition!!");
                    }
                }

            }
        }
    }
}