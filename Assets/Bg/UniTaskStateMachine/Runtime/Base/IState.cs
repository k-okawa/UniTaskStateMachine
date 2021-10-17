using System.Threading;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif


namespace Bg.UniTaskStateMachine
{
    public interface IState
    {
        void Init(BaseNode baseNode);
        UniTask OnEnter(CancellationToken ct = default);
        UniTask OnUpdate(CancellationToken ct = default);
        UniTask OnExit(CancellationToken ct = default);
    }
}