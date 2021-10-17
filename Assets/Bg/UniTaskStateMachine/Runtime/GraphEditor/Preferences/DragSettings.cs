using UnityEngine;

namespace Bg.UniTaskStateMachine
{
    [System.Serializable]
    public class DragSettings
    {
        [SerializeField] private Vector2 dragOffset = Vector2.zero;

        public Vector2 DragOffset
        {
            get => dragOffset;
            set => dragOffset = value;
        }
    }
}