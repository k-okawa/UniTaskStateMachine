using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Bg.StateMachine
{
    public class BaseNode
    {
        private BaseState State;
        private bool isUpdate = true;
        private CancellationTokenSource cancellationTokenSource;

        public bool IsUpdate
        {
            get => isUpdate;
            set
            {
                isUpdate = value;
                State.IsUpdate = isUpdate;
            }
        }

        public List<BaseCondition> Conditions = new List<BaseCondition>();

        public async UniTask<BaseNode> Start()
        {
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();

            await State.OnEnter(cancellationTokenSource.Token);
            bool isFinishUpdate = false;
            State.OnUpdate(cancellationTokenSource.Token).ContinueWith(() =>
            {
                isFinishUpdate = true;
            }).Forget();
            BaseCondition nextCondition = null;
            while (true)
            {
                await UniTask.DelayFrame(1, cancellationToken: cancellationTokenSource.Token);
                await UniTask.WaitUntil(() => IsUpdate, cancellationToken: cancellationTokenSource.Token);
                if (isFinishUpdate)
                {
                    nextCondition = CheckCondition();
                    break;
                }
                nextCondition = CheckCondition();
                if (nextCondition != null)
                {
                    break;
                }
            }
            await State.OnExit(cancellationTokenSource.Token);
            return nextCondition?.NextNode;
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

        public void Stop()
        {
            cancellationTokenSource?.Dispose();
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