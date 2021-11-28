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
                    baseNode.StateMachine.Stop();
                }
            }

            public override async UniTask OnExit(CancellationToken ct = default)
            {

            }
        }

        [UnityTest]
        public IEnumerator TimeCheck()
        {
            StateMachine sm = new StateMachine();
            var startNode = new BaseNode(sm, "Start", new PlayState());
            sm.EntryNode = startNode;
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
        
        [UnityTest]
        public IEnumerator StateMachineStartStartTest() {
            StateMachine sm = new StateMachine();
            var startNode = new BaseNode(sm, "Start", new PlayState());
            sm.EntryNode = startNode;
            sm.Start();
            
            yield return new WaitForSeconds(1.5f);
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

        [UnityTest]
        public IEnumerator StateMachinePauseResumeTest() {
            StateMachine sm = new StateMachine();
            var startNode = new BaseNode(sm, "Start", new PlayState());
            sm.EntryNode = startNode;
            sm.Start();

            yield return new WaitForSeconds(0.5f);
            sm.Pause();
            yield return new WaitForSeconds(1.5f);
            sm.Resume();

            while (true)
            {
                yield return null;
                if (sm.CurrentState == StateMachine.State.STOP)
                {
                    break;
                }
            }
        }
        
        [UnityTest]
        public IEnumerator StateMachineStopStartTest() {
            StateMachine sm = new StateMachine();
            var startNode = new BaseNode(sm, "Start", new PlayState());
            sm.EntryNode = startNode;
            sm.Start();

            yield return new WaitForSeconds(0.5f);
            sm.Stop();
            yield return new WaitForSeconds(1.5f);
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
        
        [UnityTest]
        public IEnumerator NodePauseResumeTest() {
            StateMachine sm = new StateMachine();
            var startNode = new BaseNode(sm, "Start", new PlayState());
            sm.EntryNode = startNode;
            sm.Start();

            yield return new WaitForSeconds(0.5f);
            sm.Pause();
            yield return new WaitForSeconds(1.5f);
            sm.Resume();

            while (true)
            {
                yield return null;
                if (sm.CurrentState == StateMachine.State.STOP)
                {
                    break;
                }
            }
        }
        
        [UnityTest]
        public IEnumerator NodeStopStartTest() {
            StateMachine sm = new StateMachine();
            var startNode = new BaseNode(sm, "Start", new PlayState());
            sm.EntryNode = startNode;
            sm.Start();

            yield return new WaitForSeconds(0.5f);
            sm.Stop();
            yield return new WaitForSeconds(1.5f);
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
