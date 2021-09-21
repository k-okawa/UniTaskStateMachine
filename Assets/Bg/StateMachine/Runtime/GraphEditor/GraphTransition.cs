using UnityEngine;
using UnityEngine.Serialization;

namespace Bg.StateMachine
{
    [System.Serializable]
    public class GraphTransition
    {
        [SerializeField, FormerlySerializedAs("name")]
        private string id = string.Empty;
        
        [SerializeField, FormerlySerializedAs("origin")] 
        private string originStateID = string.Empty;

        [SerializeField, FormerlySerializedAs("target")]
        private string targetStateID = string.Empty;
        
        public string ID
        {
            get => this.id;
            set => this.id = value;
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
    }
}