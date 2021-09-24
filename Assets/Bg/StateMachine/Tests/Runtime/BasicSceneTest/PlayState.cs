﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Bg.StateMachine.Tests.BasicSceneTest
{
    public class PlayState : BaseStateComponent
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
}