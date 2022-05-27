using System.Threading;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif

namespace Bg.UniTaskStateMachine
{
    public class BaseState : IState
    {
        protected BaseNode baseNode;

        public void Init(BaseNode baseNode)
        {
            this.baseNode = baseNode;
        }
        
        public virtual UniTask OnEnter(CancellationToken ct = default)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnUpdate(CancellationToken ct = default)
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnExit(CancellationToken ct = default)
        {
            return UniTask.CompletedTask;
        }
    }
}