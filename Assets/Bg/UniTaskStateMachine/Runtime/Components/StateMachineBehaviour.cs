using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif
using UnityEngine;

namespace Bg.UniTaskStateMachine
{
    [DefaultExecutionOrder(-1)]
    [DisallowMultipleComponent]
    public class StateMachineBehaviour : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Graph graph = new Graph();
        [SerializeField] 
        public bool playOnEnable = true;
        [SerializeField] 
        private PlayerLoopTiming loopTiming = PlayerLoopTiming.Update;

        public Graph Graph => graph;
        
        public StateMachine StateMachine { get; private set; }

        private void Awake()
        {
            StateMachine = new StateMachine();
            StateMachine.LoopTiming = loopTiming;
            if (Graph.EntryStateId == string.Empty)
            {
                Debug.LogError("Entry state ID not set");
                return;
            }
            
            List<BaseNode> nodes = new List<BaseNode>();
            foreach (var node in Graph.Nodes)
            {
                nodes.Add(CreateNode(node));
            }

            foreach (var transition in Graph.Transitions)
            {
                var originNode = nodes.FirstOrDefault(itr => itr.Id == transition.OriginStateID);
                var targetNode = nodes.FirstOrDefault(itr => itr.Id == transition.TargetStateID);

                if (originNode == null || targetNode == null)
                {
                    continue;
                }

                originNode.TryAddCondition(new BaseCondition(targetNode, CreateConditionFunc(transition), transition.ID)
                {
                    isNegative = transition.IsNegative
                });
            }

            var entryNode = nodes.FirstOrDefault(itr => itr.Id == Graph.EntryStateId);
            if (entryNode == null)
            {
                Debug.LogError("Entry state ID node is not found");
                return;
            }

            StateMachine.EntryNode = entryNode;
        }

        private void OnEnable()
        {
            if (StateMachine.EntryNode != null)
            {
                switch (StateMachine.CurrentState)
                {
                    case StateMachine.State.STOP:
                        if(playOnEnable) StateMachine.Start();
                        break;
                    case StateMachine.State.PAUSE:
                        StateMachine.Resume();
                        break;
                }
            }
        }

        private void OnDisable()
        {
            if (StateMachine.CurrentNode != null)
            {
                StateMachine.Pause();
            }
        }

        private void OnDestroy() {
            if (StateMachine.CurrentNode != null) {
                StateMachine.Stop();
            }
        }

        private BaseNode CreateNode(GraphNode node) 
        {
            IState iState = new BaseState();
            if (node is GraphState state)
            {
                if (state.StateComponent != null)
                {
                    iState = state.StateComponent;
                }
            }

            BaseNode retNode = new BaseNode(StateMachine, node.ID, iState);

            return retNode;
        }

        delegate bool ConditionFuncDelegate();
        private Func<bool> CreateConditionFunc(GraphTransition transition)
        {
            string methodName = transition.ConditionMethodName;
            MethodInfo methodInfo = null;
            Component comp = null;
            if (methodName != string.Empty && methodName != "None")
            {
                var divided = methodName.Split('/');
                string className = divided[0];
                string method = divided[1];
                comp = GetComponent(className);
                Type type = comp.GetType();
                methodInfo = type.GetMethod(method);
            }

            ConditionFuncDelegate conditionFunc = null;
            if (comp != null && methodInfo != null)
            {
                conditionFunc = (ConditionFuncDelegate) Delegate.CreateDelegate(typeof(ConditionFuncDelegate), comp, methodInfo.Name);
            }
            
            bool ConditionFunc() 
            {
                if (conditionFunc != null)
                {
                    bool isMatch = conditionFunc();
                    return isMatch;
                }

                return false;
            }

            return ConditionFunc;
        }
    }
}