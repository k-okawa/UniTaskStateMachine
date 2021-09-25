using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bg.UniTaskStateMachine
{
    [System.Serializable]
    public class GraphNode
    {
        private static readonly int Width = 300;
        private static readonly int Height = 100;

        [SerializeField] private Vector2 position;
        
        [SerializeField]
        private string id = String.Empty;

        public string ID
        {
            get => this.id;
            set => this.id = value;
        }
        
        /// <summary>
        /// Gets or sets the rect of the node.
        /// Remark: The setter will only apply the center of the position
        /// </summary>
        public Rect Rect
        {
            get => new Rect()
            {
                x = Position.x - Width / 2.0f,
                y = Position.y - Height / 2.0f,
                width = Width,
                height = Height
            };

            set => Position = value.center;
        }

        /// <summary>
        /// Gets or sets the position (center) of the node in the graph
        /// </summary>
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }
    }
}