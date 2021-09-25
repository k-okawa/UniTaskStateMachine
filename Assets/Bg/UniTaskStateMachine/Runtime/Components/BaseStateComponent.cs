using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bg.UniTaskStateMachine
{
    public class BaseStateComponent : MonoBehaviour , IState
    {
        protected BaseNode baseNode;
        
        public virtual void Init(BaseNode baseNode)
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