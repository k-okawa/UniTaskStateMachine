using UnityEngine;

namespace Bg.UniTaskStateMachine.Editor
{
    public class Line
    {
        public Vector2 Start { get; private set; }
        public Vector2 End { get; private set; }

        public Vector2 Direction => End - Start;

        public Line(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }

        public float GetMinDistance(Vector2 point)
        {
            return GetMinDistanceToLine(point, this);
        }
        
        public Vector2 Lerp(float t)
        {
            t = Mathf.Clamp01(t);

            return Vector2.Lerp(this.Start, this.End, t);
        }

        public bool Intersects(Rect rect)
        {
            //Create border lines of the rect
            Line leftBorder = new Line(rect.min, new Vector2(rect.xMin, rect.yMax));
            Line topBorder = new Line(rect.min, new Vector2(rect.xMax, rect.yMin));
            Line rightBorder = new Line(rect.max, new Vector2(rect.xMax, rect.yMin));
            Line bottomBorder = new Line(rect.max, new Vector2(rect.xMin, rect.yMax));

            return (rect.Contains(this.Start) && rect.Contains(this.End) ||
                    Intersects(leftBorder) || Intersects(rightBorder) ||
                    Intersects(topBorder) || Intersects(bottomBorder));
        }
        
        private bool Intersects(Line other)
        {
            return IsIntersecting(this, other);
        }
        
        private static bool IsIntersecting(Line lineA, Line lineB)
        {
            Vector2 directionA = lineA.Direction;
            Vector2 directionB = lineB.Direction;

            float det = directionB.x * directionA.y - directionA.x * directionB.y;

            if (!Mathf.Approximately(det, 0))
            {
                float deltaX = lineA.Start.x - lineB.Start.x;
                float deltaY = lineA.Start.y - lineB.Start.y;

                float s = (1 / det) * (deltaX * directionA.y - deltaY * directionA.x);
                float t = (1 / det) * (deltaX * directionB.y - deltaY * directionB.x);

                if (s > 0 && s < 1 && t > 0 && t < 1)
                {
                    return true;
                }
            }

            return false;
        }

        private static float GetMinDistanceToLine(Vector2 point, Line line)
        {
            Vector2 v = line.Direction;
            Vector2 w = point - line.Start;
            Vector2 distance = w - (1.0f / v.magnitude) * Vector2.Dot(v, w) * (v / v.magnitude);

            return distance.magnitude;
        }
    }
}