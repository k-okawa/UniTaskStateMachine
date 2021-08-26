namespace Bg.StateMachine
{
    public abstract class BaseCondition
    {
        public BaseNode NextNode;
        public abstract bool IsMatchCondition();
    }
}