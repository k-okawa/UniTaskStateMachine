using System.Collections.Generic;
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
        public string Id;
        public IState State = new BaseState();
        private CancellationTokenSource cancellationTokenSource;

        public bool IsUpdate { get; set; } = true;

        public List<BaseCondition> Conditions = new List<BaseCondition>();

        public async UniTask<BaseNode> Start(PlayerLoopTiming loopTiming = PlayerLoopTiming.Update) 
        {
            IsUpdate = true;
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            State.Init(this);
            await State.OnEnter(cancellationTokenSource.Token);
            BaseCondition nextCondition = null;
            while (true)
            {
                await UniTask.Yield(loopTiming, cancellationTokenSource.Token);
                while (!IsUpdate) {
                    await UniTask.Yield(loopTiming, cancellationTokenSource.Token);
                }
                await State.OnUpdate(cancellationTokenSource.Token);
                nextCondition = CheckCondition();
                if (nextCondition != null)
                {
                    break;
                }
            }
            await State.OnExit(cancellationTokenSource.Token);
            return nextCondition.NextNode;
        }

        private BaseCondition CheckCondition()
        {
            foreach (var condition in Conditions)
            {
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
    }
}