using System;
using System.Collections;
using System.Threading;
using UnityEngine;
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
            public override UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Init Start");
                return UniTask.CompletedTask;
            }

            public override async UniTask OnUpdate(CancellationToken ct = default)
            {
                await UniTask.DelayFrame(1, cancellationToken: ct);
                GameManager.isInit = true;
            }

            public override UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Init End");
                return UniTask.CompletedTask;
            }
        }

        public class PlayState : BaseState
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

        public class EndState : BaseState
        {
            public override UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Game End");
                baseNode.StateMachine.Stop();
                return UniTask.CompletedTask;
            }
        }
        
        [UnityTest]
        public IEnumerator SimpleTest()
        {
            StateMachine sm = new StateMachine();
            var startNode = new BaseNode(sm,"Start", new StartState());
            var playNode = new BaseNode(sm, "Play", new PlayState());
            var endNode = new BaseNode(sm, "End", new EndState());

            startNode.TryAddCondition(new BaseCondition(playNode, () => GameManager.isInit, "IsStart"));
            playNode.TryAddCondition(new BaseCondition(endNode, () => GameManager.bossHp <= 0, "IsEnd"));
            sm.EntryNode = startNode;
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
