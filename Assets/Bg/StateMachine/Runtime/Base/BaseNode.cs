using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Bg.StateMachine
{
    public class BaseNode
    {
        public BaseState State;
        private CancellationTokenSource cancellationTokenSource;

        public bool IsUpdate { get; set; } = true;

        public List<BaseCondition> Conditions = new List<BaseCondition>();

        public async UniTask<BaseNode> Start()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource = new CancellationTokenSource();

            State.Init(this);
            await State.OnEnter(cancellationTokenSource.Token);
            if (Conditions.Count <= 0)
            {
                await State.OnExit(cancellationTokenSource.Token);
                return null;
            }
            BaseCondition nextCondition = null;
            while (true)
            {
                await UniTask.DelayFrame(1, cancellationToken: cancellationTokenSource.Token);
                await UniTask.WaitUntil(() => IsUpdate, cancellationToken: cancellationTokenSource.Token);
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
                if (condition.IsMatchCondition())
                {
                    return condition;
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