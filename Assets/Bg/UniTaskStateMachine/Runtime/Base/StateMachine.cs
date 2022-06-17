using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public BaseNode EntryNode;
        public BaseNode CurrentNode { get; private set; }
        public State CurrentState { get; private set; } = State.STOP;
        public PlayerLoopTiming LoopTiming = PlayerLoopTiming.Update;

        /// <summary>
        /// Start StateMachine
        /// </summary>
        public async void Start()
        {
            if (CurrentState != State.STOP) 
            {
                return;
            }
            
            if (CurrentNode == null) 
            {
                CurrentNode = EntryNode;
                if (CurrentNode == null) 
                {
                    return;
                }
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
                catch (OperationCanceledException) 
                {
                    CurrentState = State.STOP;
                    return;
                }
            }
        }
        
        /// <summary>
        /// restart state machine from entry state
        /// </summary>
        public async UniTask ReStart(CancellationToken ct = default) 
        {
            Stop();
            await UniTask.Yield(LoopTiming, ct);
            Start();
        }

        /// <summary>
        /// force transition to next state
        /// </summary>
        /// <param name="transitionId">transition id named on graph editor</param>
        public void TriggerNextTransition(string transitionId) 
        {
            if (CurrentState != State.START) 
            {
                return;
            }

            var targetCondition = CurrentNode?.GetCondition(transitionId);
            if (targetCondition?.NextNode == null)
            {
                return;
            }

            targetCondition.isForceTransition = true;
        }

        /// <summary>
        /// whether current state is equivalent
        /// </summary>
        /// <param name="type">state type</param>
        /// <returns>return true if current state is type argument</returns>
        public bool IsMatchCurrentStateType(Type type) 
        {
            return CurrentNode?.IsMatchState(type) ?? false;
        }

        /// <summary>
        /// almost same with IsMatchCurrentStateType
        /// difference is variable length arguments
        /// </summary>
        /// <param name="types">state types</param>
        /// <returns>return true if current state match with any type arguments</returns>
        public bool IsMatchAnyCurrentStateType(params Type[] types) 
        {
            return CurrentNode?.IsMatchAnyState(types) ?? false;
        }

        /// <summary>
        /// stop state machine completely
        /// </summary>
        public void Stop()
        {
            if (CurrentState == State.STOP) 
            {
                return;
            }
            CurrentNode?.Stop();
            CurrentNode = EntryNode;
        }

        /// <summary>
        /// pause current state
        /// </summary>
        public void Pause()
        {
            if (CurrentState != State.START) 
            {
                return;
            }
            CurrentState = State.PAUSE;
            CurrentNode?.Pause();
        }

        /// <summary>
        /// resume current state
        /// </summary>
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