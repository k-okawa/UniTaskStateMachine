using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Bg.StateMachine.Editor
{
    public class ZoomSlider : ToolbarElement
    {
        public ZoomSlider(EditorWindow window) : base(window)
        {
            this.Width = 185;
        }

        public override void OnGUI(Rect rect)
        {
            EditorGUI.BeginDisabledGroup(false);
            {
                var zoomSliderRect = new Rect((rect.width - this.Width - 50) / 2 + 50, rect.y, this.Width, rect.height);
                
                GUI.Label(new Rect((rect.width - this.Width - 50) / 2, rect.y, 50, rect.height), "Zoom");

                Context.ZoomFactor = GUI.HorizontalSlider(zoomSliderRect, Context.ZoomFactor, ZoomSettings.MinZoomFactor, ZoomSettings.MaxZoomFactor);
            }
            EditorGUI.EndDisabledGroup();
            
            //Zoom in/out when the scroll wheel has been moved
            if (Event.current.type == EventType.ScrollWheel)
            {
                Context.ZoomFactor -= Mathf.Sign(Event.current.delta.y) * ZoomSettings.MaxZoomFactor / 20.0f;

                Event.current.Use();
            }
        }
    }
}