using System;

namespace Bg.StateMachine
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