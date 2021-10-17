using UnityEngine;

namespace Bg.UniTaskStateMachine
{
    [System.Serializable]
    public class Preferences
    {
        [SerializeField]
        private ZoomSettings zoomSettings = new ZoomSettings();

        [SerializeField]
        private DragSettings dragSettings = new DragSettings();
        
        public ZoomSettings ZoomSettings { get => this.zoomSettings; set => this.zoomSettings = value; }
        public DragSettings DragSettings { get => this.dragSettings; set => this.dragSettings = value; }
    }
}