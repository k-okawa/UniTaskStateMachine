using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class StateStyles
    {
        public enum Style
        {
            Normal = 0,
            Blue = 1,
            Mint = 2,
            Green = 3,
            Yellow = 4,
            Orange = 5,
            Red = 6,
            NormalOn = 7,
            BlueOn = 8,
            MintOn = 9,
            GreenOn = 10,
            YellowOn = 11,
            OrangeOn = 12,
            RedOn = 13
        }

        private Dictionary<int, GUIStyle> styleDictionary = null;

        public StateStyles()
        {
            styleDictionary = new Dictionary<int, GUIStyle>();

            for (int i = 0; i <= 6; i++)
            {
                styleDictionary.Add(i, CreateStateStyle("builtin skins/darkskin/images/node"+i.ToString()+".png"));
                styleDictionary.Add(i+7, CreateStateStyle("builtin skins/darkskin/images/node" + i.ToString() + " on.png"));
            }
        }

        public GUIStyle Get(Style style)
        {
            return styleDictionary[(int) style];
        }

        GUIStyle CreateStateStyle(string builtinTexturePath)
        {
            GUIStyle style = new GUIStyle
            {
                border = new RectOffset(10, 10, 10, 10),
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                contentOffset = new Vector2(0, -3)
            };
            
            style.normal.background = EditorGUIUtility.Load(builtinTexturePath) as Texture2D;
            style.normal.textColor = Color.white;

            return style;
        }

        public void ApplyZoomFactor(float zoomFactor)
        {
            foreach (GUIStyle style in styleDictionary.Values)
            {
                style.fontSize = Mathf.RoundToInt(20 * zoomFactor);
                style.contentOffset = new Vector2(0, -3.0f * zoomFactor);
            }
        }
    }
}