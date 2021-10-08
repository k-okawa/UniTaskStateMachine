using System.Threading;
using Cysharp.Threading.Tasks;

namespace Bg.UniTaskStateMachine.Tests.BasicSceneTest
{
    public class StartState : BaseStateComponent
    {
        public override async UniTask OnEnter(CancellationToken ct = default)
        {
            UnityEngine.Debug.Log("Init Start");
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