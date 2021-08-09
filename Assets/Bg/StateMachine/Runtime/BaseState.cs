using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Bg.StateMachine
{
    public class BaseState
    {
        public bool IsUpdate = true;
        
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