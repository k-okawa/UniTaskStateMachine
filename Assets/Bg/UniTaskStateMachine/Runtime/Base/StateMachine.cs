using System;
using System.Linq;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif

namespace Bg.UniTaskStateMachine
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
        public PlayerLoopTiming LoopTiming = PlayerLoopTiming.Update;

        public async void Start()
        {
            if (CurrentState != State.STOP) 
            {
                return;
            }
            
            if (CurrentNode == null)
            {
                return;
            }

            CurrentState = State.START;
            while (true)
            {
                try 
                {
                    var nextNode = await CurrentNode.Start(LoopTiming);
                    if (nextNode == null) {
                        CurrentState = State.STOP;
                        return;
                    }
                    CurrentNode = nextNode;
                }
                catch (OperationCanceledException e) 
                {
                    CurrentState = State.STOP;
                    return;
                }
            }
        }

        public async UniTask TriggerNextTransition(string transitionId) 
        {
            if (CurrentState != State.START) 
            {
                return;
            }

            if (CurrentNode == null) 
            {
                return;
            }

            var targetCondition = CurrentNode.Conditions.FirstOrDefault(itr => itr.TransitionId == transitionId);
            if (targetCondition?.NextNode == null) 
            {
                return;
            }

            CurrentNode.IsUpdate = false;

            await CurrentNode.State.OnExit();
            
            Stop();

            CurrentNode = targetCondition.NextNode;
            
            Start();
        }

        public void Stop()
        {
            if (CurrentState == State.STOP) 
            {
                return;
            }
            CurrentState = State.STOP;
            CurrentNode?.Stop();
        }

        public void Pause()
        {
            if (CurrentState != State.START) 
            {
                return;
            }
            CurrentState = State.PAUSE;
            CurrentNode?.Pause();
        }

        public void Resume()
        {
            if (CurrentState != State.PAUSE) 
            {
                return;
            }
            CurrentState = State.START;
            CurrentNode?.Resume();
        }
    }
}