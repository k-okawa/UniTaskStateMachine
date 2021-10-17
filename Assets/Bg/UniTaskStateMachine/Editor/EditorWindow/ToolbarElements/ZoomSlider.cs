using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class ZoomSlider : ToolbarElement
    {
        public ZoomSlider(EditorWindow window) : base(window)
        {
            this.Width = 160;
        }

        public override void OnGUI(Rect rect)
        {
            if (!Context.IsStateMachineLoaded)
            {
                return;
            }
            
            EditorGUI.BeginDisabledGroup(false);
            {
                var zoomSliderRect = new Rect((rect.width - this.Width - 50) + 40, rect.y, this.Width, rect.height);
                
                GUI.Label(new Rect((rect.width - this.Width - 50), rect.y, 40, rect.height), "Zoom");

                Context.ZoomFactor = GUI.HorizontalSlider(zoomSliderRect, Context.ZoomFactor, ZoomSettings.MinZoomFactor, ZoomSettings.MaxZoomFactor);
            }
            EditorGUI.EndDisabledGroup();
            
            //Zoom in/out when the scroll wheel has been moved
            if (Event.current.type == EventType.ScrollWheel)
            {
                Context.ZoomFactor -= Mathf.Sign(Event.current.delta.y) * ZoomSettings.MaxZoomFactor / 30.0f;

                Event.current.Use();
            }
        }
    }
}