using System.Collections.Generic;
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
        
        [System.NonSerialized]
        private StateMachineBehaviour stateMachine = new StateMachineBehaviour();

        public StateMachineBehaviour StateMachine
        {
            get => stateMachine;
        }

        public Graph Graph
        {
            get => stateMachine.Graph;
        }

        public List<GraphNode> SelectedNodes { get; } = new List<GraphNode>();
    }
}