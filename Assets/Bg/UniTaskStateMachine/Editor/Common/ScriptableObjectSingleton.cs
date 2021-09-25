using System;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var ins = Resources.FindObjectsOfTypeAll<T>();

                    if (ins.Length > 0)
                    {
                        return ins[0];
                    }

                    instance = CreateInstance<T>();
                }

                return instance;
            }
        }

        protected void OnDisable()
        {
            instance = null;
        }
    }
}