using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.Assertions;
#if BG_USE_UNIRX_ASYNC
using UniRx.Async;
#else
using Cysharp.Threading.Tasks;
#endif
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Bg.UniTaskStateMachine.Tests
{
    public class StateMachineLoopTest
    {
        public static float time = 0f;
        
        public class PlayState : BaseState {
            public override async UniTask OnEnter(CancellationToken ct = default) 
            {
                time = 0f;
            }

            public override async UniTask OnUpdate(CancellationToken ct = default) 
            {
                time += Time.deltaTime;
                Debug.Log($"Time:{time}");
                if (time >= 2f) {
                    baseNode.Stop();
                }
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {

            }
        }

        [UnityTest]
        public IEnumerator TimeCheck()
        {
            var startNode = new BaseNode();
            startNode.State = new PlayState();

            StateMachine sm = new StateMachine();
            sm.CurrentNode = startNode;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            sm.Start();

            while (true)
            {
                yield return null;
                if (sm.CurrentState == StateMachine.State.STOP)
                {
                    sp.Stop();
                    Debug.Log($"ActualTime:{sp.Elapsed.TotalSeconds}");
                    Assert.AreApproximatelyEqual((float)sp.Elapsed.TotalSeconds, time, 0.02f);
                    break;
                }
            }
        }
    }
}
