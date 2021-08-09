using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bg.StateMachine
{
    public class StateMachine
    {
        public BaseNode CurrentNode;

        public async void Start()
        {
            if (CurrentNode == null)
            {
                return;
            }

            while (true)
            {
                var nextNode = await CurrentNode.Start();
                if (nextNode == null)
                {
                    return;
                }
                CurrentNode = nextNode;
            }
        }

        public void Stop()
        {
            CurrentNode?.Stop();
        }

        public void Pause()
        {
            CurrentNode?.Pause();
        }

        public void Resume()
        {
            CurrentNode?.Resume();
        }
    }
}
