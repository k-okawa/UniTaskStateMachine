using System;

namespace Bg.UniTaskStateMachine
{
    public class BaseCondition
    {
        public BaseNode NextNode { get; private set; }
        public Func<bool> ConditionCheckCallback { get; private set; }
        public string TransitionId { get; private set; }
        public bool isNegative = false;

        public BaseCondition(BaseNode nextNode, Func<bool> conditionCheckCallback, string transitionId)
        {
            this.NextNode = nextNode;
            this.ConditionCheckCallback = conditionCheckCallback;
            TransitionId = transitionId;
        }
    }
}