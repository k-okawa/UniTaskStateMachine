using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bg.StateMachine
{
    public class BaseStateComponent : MonoBehaviour , IState
    {
        protected BaseNode baseNode;
        
        public void Init(BaseNode baseNode)
        {
            this.baseNode = baseNode;
        }

        public async UniTask OnEnter(CancellationToken ct = default)
        {
            
        }

        public async UniTask OnUpdate(CancellationToken ct = default)
        {
            
        }

        public async UniTask OnExit(CancellationToken ct = default)
        {
            
        }
    }
}