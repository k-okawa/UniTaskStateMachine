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
    public class BaseNode
    {
        public readonly string Id;
        public readonly StateMachine StateMachine;
        public bool IsUpdate { get; private set; } = true;
        
        private readonly IState State;
        private List<BaseCondition> conditions = new List<BaseCondition>();
        private CancellationTokenSource cancellationTokenSource;
        
        public BaseNode(StateMachine stateMachine, string id, IState state)
        {
            this.Id = id;
            this.State = state;
            this.StateMachine = stateMachine;
        }

        internal async UniTask<BaseNode> Start(PlayerLoopTiming loopTiming = PlayerLoopTiming.Update) 
        {
            IsUpdate = true;
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            foreach (var condition in conditions)
            {
                condition.isForceTransition = false;
            }
            State.Init(this);
            await State.OnEnter(cancellationTokenSource.Token);
            BaseCondition nextCondition = CheckCondition();
            while (nextCondition == null)
            {
                await UniTask.Yield(loopTiming, cancellationTokenSource.Token);
                while (!IsUpdate) {
                    await UniTask.Yield(loopTiming, cancellationTokenSource.Token);
                }
                await State.OnUpdate(cancellationTokenSource.Token);
                nextCondition = CheckCondition();
            }
            await State.OnExit(cancellationTokenSource.Token);
            return nextCondition.NextNode;
        }

        private BaseCondition CheckCondition()
        {
            foreach (var condition in conditions)
            {
                if (condition.isForceTransition)
                {
                    return condition;
                }
                
                if (condition.isNegative) 
                {
                    if (!condition.ConditionCheckCallback?.Invoke() ?? false)
                    {
                        return condition;
                    }
                } else 
                {
                    if (condition.ConditionCheckCallback?.Invoke() ?? false)
                    {
                        return condition;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// is match any condition
        /// </summary>
        /// <returns>return true if node has any true conditions</returns>
        public bool IsMatchAnyCondition()
        {
            return CheckCondition() != null;
        }

        /// <summary>
        /// is exist force transition
        /// </summary>
        /// <returns>return true if node has any force transition</returns>
        public bool IsExistForceTransition()
        {
            return conditions.Any(itr => itr.IsForceTransition);
        }

        public bool IsMatchState(Type type)
        {
            return State.GetType() == type;
        }

        public bool IsMatchAnyState(params Type[] types) 
        {
            return types.Any(IsMatchState);
        }

        internal void Stop() 
        {
            IsUpdate = false;
            cancellationTokenSource?.Cancel();
        }

        internal void Pause()
        {
            IsUpdate = false;
        }

        internal void Resume()
        {
            IsUpdate = true;
        }

        public bool TryAddCondition(BaseCondition condition)
        {
            if (conditions.Any(itr => itr.TransitionId == condition.TransitionId))
            {
                return false;
            }
            
            conditions.Add(condition);

            return true;
        }
        
        /// <summary>
        /// get condition
        /// </summary>
        /// <param name="id">condition id (transition id named on graph editor)</param>
        /// <returns>base condition</returns>
        public BaseCondition GetCondition(string id)
        {
            return conditions.FirstOrDefault(itr => itr.TransitionId == id);
        }
        
        /// <summary>
        /// get transition ids that node has
        /// </summary>
        public IEnumerable<string> GetTransitionIds()
        {
            return conditions.Select(itr => itr.TransitionId);
        }
    }
}