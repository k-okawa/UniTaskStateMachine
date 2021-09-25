using System;

namespace Bg.UniTaskStateMachine
{
    public class BaseCondition
    {
        public BaseNode NextNode { get; private set; }
        public Func<bool> ConditionCheckCallback { get; private set; }

        public BaseCondition(BaseNode nextNode, Func<bool> conditionCheckCallback)
        {
            this.NextNode = nextNode;
            this.ConditionCheckCallback = conditionCheckCallback;
        }
    }
}