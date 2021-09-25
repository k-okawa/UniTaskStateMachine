namespace Bg.UniTaskStateMachine.Editor.Extensions
{
    public static class StateMachineHelper
    {
        public static Graph GetStateMachineGraph(this StateMachineBehaviour stateMachine)
        {
            return stateMachine.Graph;
        }
        
        public static Preferences GetPreferences(this StateMachineBehaviour stateMachine)
        {
            return stateMachine.Graph.Preferences;
        }
    }
}