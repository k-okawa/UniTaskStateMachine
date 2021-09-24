﻿using System.Threading;
using Cysharp.Threading.Tasks;

namespace Bg.StateMachine.Tests.BasicSceneTest
{
    public class EndState : BaseStateComponent
    {
        public override async UniTask OnEnter(CancellationToken ct = default)
        {
            UnityEngine.Debug.Log("Game End");
        }

        public override UniTask OnUpdate(CancellationToken ct = default)
        {
            return base.OnUpdate(ct);
        }

        public override UniTask OnExit(CancellationToken ct = default)
        {
            return base.OnExit(ct);
        }
    }
}