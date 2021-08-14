namespace Bg.StateMachine
{
    public class StateMachine
    {
        public enum State
        {
            STOP,
            START,
            PAUSE
        }
        
        public BaseNode CurrentNode;
        public State CurrentState { get; private set; } = State.STOP;

        public async void Start()
        {
            if (CurrentNode == null)
            {
                return;
            }

            CurrentState = State.START;
            while (true)
            {
                var nextNode = await CurrentNode.Start();
                if (nextNode == null)
                {
                    CurrentState = State.STOP;
                    return;
                }
                CurrentNode = nextNode;
            }
        }

        public void Stop()
        {
            CurrentState = State.STOP;
            CurrentNode?.Stop();
        }

        public void Pause()
        {
            CurrentState = State.PAUSE;
            CurrentNode?.Pause();
        }

        public void Resume()
        {
            CurrentState = State.START;
            CurrentNode?.Resume();
        }
    }
}
