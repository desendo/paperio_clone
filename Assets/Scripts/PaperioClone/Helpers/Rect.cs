using UnityEngine;

namespace PaperIOClone.Helpers
{
    public class Rect
    {
        public float Bottom;
        public float Left;
        public float Right;
        public float Top;

        public bool Overlaps(Rect rect, float overlap = 0)
        {
            if (Left - overlap > rect.Right + overlap || rect.Left - overlap > Right + overlap)
                return false;
            if (Top + overlap < rect.Bottom - overlap || rect.Top + overlap < Bottom - overlap)
                return false;
            return true;
        }

        public bool Overlaps(Vector2 point, float overlap = 0)
        {
            if (Left - overlap > point.x + overlap || point.x - overlap > Right + overlap)
                return false;
            if (Top + overlap < point.y - overlap || point.y + overlap < Bottom - overlap)
                return false;
            return true;
        }

        internal void InitWithPosition(Vector2 pos)
        {
            Left = pos.x;
            Right = pos.x;
            Top = pos.y;
            Bottom = pos.y;
        }

        internal void UpdateWithPosition(Vector2 pos)
        {
            if (pos.x < Left)
                Left = pos.x;
            else if (pos.x > Right)
                Right = pos.x;

            if (pos.y < Bottom)
                Bottom = pos.y;
            else if (pos.y > Top)
                Top = pos.y;
        }

        internal void Reset()
        {
            Bottom = 0;
            Top = 0;
            Left = 0;
            Right = 0;
        }
    }
}