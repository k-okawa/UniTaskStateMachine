using System.Collections.Generic;
using Bg.UniTaskStateMachine.Editor.Extensions;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
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

        [SerializeField]
        private int instanceId = 0;
        
        [System.NonSerialized]
        private StateMachineBehaviour stateMachine = null;

        public StateMachineBehaviour StateMachine
        {
            get => stateMachine;
            private set
            {
                if (value != null)
                {
                    if (PrefabUtility.IsPartOfPrefabAsset(value))
                    {
                        return;
                    }

                    Graph = value.GetStateMachineGraph();
                }
                
                LoadSettings(value);

                stateMachine = value;
            }
        }

        public Graph Graph { get; private set; }

        public List<GraphNode> SelectedNodes { get; } = new List<GraphNode>();
        
        public SelectionRect SelectionRect { get; set; } = new SelectionRect();

        [System.NonSerialized]
        private GraphNode transitionPreview = null;
        
        public GraphNode TransitionPreview
        {
            get => transitionPreview;
            set => transitionPreview = value;
        }

        public bool IsStateMachineLoaded => (this.stateMachine != null);

        public bool IsPrefabAsset
        {
            get => IsStateMachineLoaded && PrefabUtility.IsPartOfAnyPrefab(StateMachine.gameObject);
        }

        [System.NonSerialized]
        public GraphSelection GraphSelection;

        public Context()
        {
            GraphSelection = new GraphSelection(this);
        }

        public void LoadStateMachine(StateMachineBehaviour stateMachine)
        {
            instanceId = (stateMachine != null) ? stateMachine.GetInstanceID() : 0;
            stateMachine = this.stateMachine;
            
            SelectedNodes.Clear();
            TransitionPreview = null;
            
            Reload();
        }

        private void LoadSettings(StateMachineBehaviour stateMachine)
        {
            if (stateMachine != null)
            {
                var preferences = stateMachine.GetPreferences();
                
                zoomSettings = preferences.ZoomSettings;
                dragSettings = preferences.DragSettings;
            }
            else
            {
                zoomSettings = new ZoomSettings();
                dragSettings = new DragSettings();
            }
        }

        public void Reload()
        {
            if (IsStateMachineLoaded == false)
            {
                if (TryFind(instanceId, out StateMachineBehaviour stateMachine))
                {
                    this.StateMachine = stateMachine;
                }
            }
        }

        private bool TryFind(int instanceID, out StateMachineBehaviour stateMachine)
        {
            stateMachine = null;

            var machines = Resources.FindObjectsOfTypeAll<StateMachineBehaviour>();

            foreach (StateMachineBehaviour machine in machines)
            {
                if (machine.GetInstanceID() == instanceID)
                {
                    stateMachine = machine;
                    return true;
                }
            }
            
            return false;
        }

        public void UpdateSelection()
        {
            if (Selection.activeGameObject != null)
            {
                GameObject selection = Selection.activeGameObject;

                var machine = selection.GetComponent<StateMachineBehaviour>();

                if (machine != null)
                {
                    LoadStateMachine(machine);
                }
            }
        }
    }
}