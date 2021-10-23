using System;

namespace Bg.UniTaskStateMachine
{
    public class BaseCondition
    {
        public BaseNode NextNode { get; }
        public Func<bool> ConditionCheckCallback { get; }
        public string TransitionId { get; }
        
        internal bool isNegative = false;
        public bool IsNegative => isNegative;

        internal bool isForceTransition = false;
        public bool IsForceTransition => isForceTransition;

        public BaseCondition(BaseNode nextNode, Func<bool> conditionCheckCallback, string transitionId)
        {
            this.NextNode = nextNode;
            this.ConditionCheckCallback = conditionCheckCallback;
            TransitionId = transitionId;
        }
    }
}