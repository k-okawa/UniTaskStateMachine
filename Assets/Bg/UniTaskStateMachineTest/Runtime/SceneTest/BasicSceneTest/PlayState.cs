using System;
using System.Threading;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif

namespace Bg.UniTaskStateMachine.Tests.BasicSceneTest
{
    public class PlayState : BaseStateComponent
    {
        public override UniTask OnEnter(CancellationToken ct = default)
        {
            UnityEngine.Debug.Log("Play Start");
            UnityEngine.Debug.Log($"Boss Hp = {GameManager.bossHp}");
            return UniTask.CompletedTask;
        }

        public override async UniTask OnUpdate(CancellationToken ct = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: ct);
            GameManager.bossHp -= 10;
            if (GameManager.bossHp < 0)
            {
                GameManager.bossHp = 0;
            }

            UnityEngine.Debug.Log($"Boss Hp = {GameManager.bossHp}");
        }

        public override UniTask OnExit(CancellationToken ct = default)
        {
            UnityEngine.Debug.Log("Play End");
            return UniTask.CompletedTask;
        }
    }
}