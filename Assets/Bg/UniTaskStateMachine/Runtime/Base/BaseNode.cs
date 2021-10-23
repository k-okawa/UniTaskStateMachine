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

        public async UniTask<BaseNode> Start(PlayerLoopTiming loopTiming = PlayerLoopTiming.Update) 
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

        public bool IsMatchAnyCondition()
        {
            return CheckCondition() != null;
        }

        public bool IsExistForceTransition()
        {
            return conditions.Any(itr => itr.IsForceTransition);
        }

        public bool IsMatchState(Type type)
        {
            return State.GetType() == type;
        }

        public void Stop() 
        {
            IsUpdate = false;
            cancellationTokenSource?.Cancel();
        }

        public void Pause()
        {
            IsUpdate = false;
        }

        public void Resume()
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
        
        public BaseCondition GetCondition(string id)
        {
            return conditions.FirstOrDefault(itr => itr.TransitionId == id);
        }

        public IEnumerable<string> GetConditionIds()
        {
            return conditions.Select(itr => itr.TransitionId);
        }
    }
}