using System.Threading;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif

namespace Bg.UniTaskStateMachine.Tests.BasicSceneTest
{
    public class StartState : BaseStateComponent
    {
        public override async UniTask OnEnter(CancellationToken ct = default)
        {
            UnityEngine.Debug.Log("Init Start");
            GameManager.bossHp = 100;
        }

        public override async UniTask OnUpdate(CancellationToken ct = default)
        {
            GameManager.progress += 1;
            if (GameManager.progress > 100)
            {
                GameManager.progress = 100;
            }
        }

        public override async UniTask OnExit(CancellationToken ct = default)
        {
            UnityEngine.Debug.Log("Init End");
        }
    }
}