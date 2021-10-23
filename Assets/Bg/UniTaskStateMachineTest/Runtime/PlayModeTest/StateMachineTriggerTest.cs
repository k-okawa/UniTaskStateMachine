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
            private StateMachine sm;
            public StartState(StateMachine stateMachine) {
                sm = stateMachine;
            }
            
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Init Start");
            }

            public override async UniTask OnUpdate(CancellationToken ct = default)
            {
                await UniTask.DelayFrame(1);
                GameManager.isInit = true;
                sm.TriggerNextTransition("IsStart");
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Init End");
            }
        }

        public class PlayState : BaseState
        {
            private StateMachine sm;
            public PlayState(StateMachine stateMachine) {
                sm = stateMachine;
            }
            
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
                    sm.TriggerNextTransition("IsEnd");
                }
                else
                {
                    sm.TriggerNextTransition("Pause");
                }
                
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Play End");
            }
        }
        
        public class PauseState : BaseState
        {
            private StateMachine sm;

            public PauseState(StateMachine stateMachine) {
                sm = stateMachine;
            }
            
            public override async UniTask OnEnter(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Pause");
            }

            public override async UniTask OnUpdate(CancellationToken ct = default)
            {
                await UniTask.DelayFrame(10, cancellationToken: ct);
                sm.TriggerNextTransition("Resume");
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {
                UnityEngine.Debug.Log("Resume");
            }
        }

        public class EndState : BaseState
        {
            private StateMachine sm;
            public EndState(StateMachine stateMachine) {
                sm = stateMachine;
            }
            
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
            
            var startNode = new BaseNode();
            startNode.State = new StartState(sm);
            var playNode = new BaseNode();
            playNode.State = new PlayState(sm);
            var pauseNode = new BaseNode();
            pauseNode.State = new PauseState(sm);
            var endNode = new BaseNode();
            endNode.State = new EndState(sm);
            
            startNode.Conditions.Add(new BaseCondition(playNode, () => false, "IsStart"));
            playNode.Conditions.Add(new BaseCondition(endNode, () => false, "IsEnd"));
            playNode.Conditions.Add(new BaseCondition(pauseNode, () => false, "Pause"));
            pauseNode.Conditions.Add(new BaseCondition(playNode, () =>false, "Resume"));
            
            sm.CurrentNode = startNode;
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
