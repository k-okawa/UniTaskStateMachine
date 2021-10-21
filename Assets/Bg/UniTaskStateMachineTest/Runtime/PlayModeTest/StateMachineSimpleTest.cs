using System;
using System.Collections;
using System.Threading;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif
using UnityEngine.TestTools;

namespace Bg.UniTaskStateMachine.Tests
{
    public class StateMachineSimpleTest
    {
        public static class GameManager
        {
            public static bool isInit = false;
            public static int bossHp = 100;
        }

        public class StartState : BaseState
        {
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Init Start");
            }

            public override async UniTask OnUpdate(CancellationToken ct = default)
            {
                await UniTask.DelayFrame(1);
                GameManager.isInit = true;
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Init End");
            }
        }

        public class PlayState : BaseState
        {
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Play Start");
                UnityEngine.Debug.Log($"Boss Hp = {GameManager.bossHp}");
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

            public override async UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Play End");
            }
        }

        public class EndState : BaseState
        {
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Game End");
            }
        }
        
        [UnityTest]
        public IEnumerator SimpleTest()
        {
            var startNode = new BaseNode();
            startNode.State = new StartState();
            var playNode = new BaseNode();
            playNode.State = new PlayState();
            var endNode = new BaseNode();
            endNode.State = new EndState();
            
            startNode.Conditions.Add(new BaseCondition(playNode, () => GameManager.isInit, "IsStart"));

            playNode.Conditions.Add(new BaseCondition(endNode, () => GameManager.bossHp <= 0, "IsEnd"));
            
            StateMachine sm = new StateMachine();
            sm.CurrentNode = startNode;
            sm.Start();

            while (true)
            {
                yield return null;
                if (sm.CurrentState == StateMachine.State.STOP)
                {
                    break;
                }
            }
        }
    }
}
