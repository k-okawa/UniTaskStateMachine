using UnityEngine;

namespace Bg.StateMachine.Editor
{
    [System.Serializable]
    public class Context
    {
        [System.NonSerialized] private ZoomSettings zoomSettings = new ZoomSettings();
        
        public float ZoomFactor
        {
            get => this.zoomSettings.ZoomFactor;
            set => this.zoomSettings.ZoomFactor = value;
        }
        
        [System.NonSerialized] private DragSettings dragSettings = new DragSettings();
        
        public Vector2 DragOffset
        {
            get => this.dragSettings.DragOffset;
            set => this.dragSettings.DragOffset = value;
        }
    }
}