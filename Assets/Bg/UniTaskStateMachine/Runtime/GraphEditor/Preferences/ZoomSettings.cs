using UnityEngine;

namespace Bg.UniTaskStateMachine
{
    [System.Serializable]
    public class ZoomSettings
    {
        public static readonly float MinZoomFactor = 0.4f;
        public static readonly float MaxZoomFactor = 1.0f;

        [SerializeField]
        private float zoomFactor = 0.7f;

        public float ZoomFactor
        {
            get => this.zoomFactor;
            set
            {
                this.zoomFactor = Mathf.Clamp(value, MinZoomFactor, MaxZoomFactor);
            }
        }
    }
}