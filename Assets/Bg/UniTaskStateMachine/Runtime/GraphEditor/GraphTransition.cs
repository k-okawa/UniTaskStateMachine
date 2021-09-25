using UnityEngine;
using UnityEngine.Serialization;

namespace Bg.UniTaskStateMachine
{
    [System.Serializable]
    public class GraphTransition
    {
        [SerializeField]
        private string id = string.Empty;

        [SerializeField]
        private string description = string.Empty;
        
        [SerializeField] 
        private string originStateID = string.Empty;

        [SerializeField]
        private string targetStateID = string.Empty;

        [SerializeField] 
        private string conditionMethodName = string.Empty;
        
        public string ID
        {
            get => this.id;
            set => this.id = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public string OriginStateID
        {
            get => originStateID;
            set => originStateID = value;
        }
        
        public string TargetStateID
        {
            get => targetStateID;
            set => targetStateID = value;
        }

        public string ConditionMethodName
        {
            get => conditionMethodName;
            set => conditionMethodName = value;
        }
    }
}