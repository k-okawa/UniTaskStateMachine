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
        private CancellationTokenSource updateCancellationTokenSource;

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
            updateCancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            updateCancellationTokenSource = new CancellationTokenSource();
            
            await State.OnEnter(cancellationTokenSource.Token);
            State.OnUpdate(updateCancellationTokenSource.Token).Forget();
            BaseCondition nextCondition = null;
            while (true)
            {
                await UniTask.DelayFrame(1, cancellationToken: cancellationTokenSource.Token);
                await UniTask.WaitUntil(() => IsUpdate, cancellationToken: cancellationTokenSource.Token);
                if (updateCancellationTokenSource.IsCancellationRequested)
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
            updateCancellationTokenSource?.Dispose();
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
            updateCancellationTokenSource?.Dispose();
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