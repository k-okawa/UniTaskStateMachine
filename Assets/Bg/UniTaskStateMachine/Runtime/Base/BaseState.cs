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
        
        public virtual async UniTask OnEnter(CancellationToken ct = default)
        {
            
        }

        public virtual async UniTask OnUpdate(CancellationToken ct = default)
        {
            
        }

        public virtual async UniTask OnExit(CancellationToken ct = default)
        {
            
        }
    }
}