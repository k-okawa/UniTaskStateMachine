namespace Bg.UniTaskStateMachine.Editor
{
    public class ErrorMessages
    {
        public static string TakenStateID { get; } = "Could not change state id '{0}' to '{1}' because the id is already taken by another state of the state machine.";

        public static string TakenTransitionName { get; } = "Could not rename transition '{0}' to '{1}' because the name is already taken by another transition of the state machine.";

    }
}