﻿using System.Threading;
using Cysharp.Threading.Tasks;

namespace Bg.StateMachine
{
    public class BaseState
    {
        protected BaseNode baseNode;

        public void Init(BaseNode baseNode)
        {
            this.baseNode = baseNode;
        }
        
        public virtual async UniTask OnEnter(CancellationToken ct = default)
        {
            
        }

        public virtual async UniTask OnUpdate(CancellationToken ct = default)
        {
            
        }

        public virtual async UniTask OnExit(CancellationToken ct = default)
        {
            
        }
    }
}