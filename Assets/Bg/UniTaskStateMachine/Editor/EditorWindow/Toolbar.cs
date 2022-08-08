using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class Toolbar
    {
        private readonly List<ToolbarElement> elements;
        
        public Toolbar(EditorWindow editorWindow)
        {
            this.elements = new List<ToolbarElement>()
            {
                new StateMachineField(editorWindow),
                new TransitionIdConstantGenerator(editorWindow),
                new ZoomSlider(editorWindow)
            };
        }

        public void OnGUI(Rect rect)
        {
            if (!Event.current.isKey)
            {
                GUI.BeginGroup(new Rect(0, 0, rect.width, EditorStyles.toolbar.fixedHeight), EditorStyles.toolbar);
                {
                    Rect toolbarRect = new Rect(0, 0, rect.width, EditorStyles.toolbar.fixedHeight);

                    foreach (var element in this.elements)
                    {
                        element.OnGUI(toolbarRect);
                    }
                }
                GUI.EndGroup();
            }
        }
    }
}