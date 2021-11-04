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
    public class StateMachineTriggerTest
    {
        public static class GameManager
        {
            public static bool isInit = false;
            public static int bossHp = 50;
        }

        public class StartState : BaseState
        {
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Init Start");
            }

            public override async UniTask OnUpdate(CancellationToken ct = default)
            {
                await UniTask.DelayFrame(1, cancellationToken: ct);
                GameManager.isInit = true;
                baseNode.StateMachine.TriggerNextTransition("IsStart");
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
                    baseNode.StateMachine.TriggerNextTransition("IsEnd");
                }
                else
                {
                    baseNode.StateMachine.TriggerNextTransition("Pause");
                }
                
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Play End");
            }
        }
        
        public class PauseState : BaseState
        {
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Pause");
            }

            public override async UniTask OnUpdate(CancellationToken ct = default)
            {
                await UniTask.DelayFrame(10, cancellationToken: ct);
                baseNode.StateMachine.TriggerNextTransition("Resume");
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Resume");
            }
        }

        public class EndState : BaseState
        {
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Game End");
                baseNode.Stop();
            }
        }
        
        [UnityTest]
        public IEnumerator TriggerTest()
        {
            StateMachine sm = new StateMachine();
            
            var startNode = new BaseNode(sm, "Start", new StartState());
            var playNode = new BaseNode(sm, "Play", new PlayState());
            var pauseNode = new BaseNode(sm, "Pause", new PauseState());
            var endNode = new BaseNode(sm, "End", new EndState());
            
            startNode.TryAddCondition(new BaseCondition(playNode, () => false, "IsStart"));
            playNode.TryAddCondition(new BaseCondition(endNode, () => false, "IsEnd"));
            playNode.TryAddCondition(new BaseCondition(pauseNode, () => false, "Pause"));
            pauseNode.TryAddCondition(new BaseCondition(playNode, () =>false, "Resume"));
            
            sm.EntryNode = startNode;
            sm.Start();

            while (true)
            {
                yield return null;
                if (sm.CurrentNode == endNode)
                {
                    break;
                }
            }
        }
    }
}
