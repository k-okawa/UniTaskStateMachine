using System.Threading;
using Cysharp.Threading.Tasks;

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