using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Tests.BasicSceneTest
{
    public class ConditionComponent : MonoBehaviour
    {
        public bool IsInitEnd()
        {
            return GameManager.progress >= 100;
        }

        public bool IsGameEnd()
        {
            return GameManager.bossHp <= 0;
        }
    }   
}
