using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    public class Rect
    {
        public float top = 0;
        public float bottom = 0;
        public float left = 0;
        public float right = 0;

        public bool Overlaps(Rect rect, float overlap = 0)
        {
            
            if (left-overlap > rect.right + overlap || rect.left - overlap > right + overlap)
                return false;
            if (top + overlap < rect.bottom - overlap || rect.top + overlap < bottom - overlap)
                return false;
            return true;
        }

        internal void InitWithPosition(Vector2 pos)
        {
            left = pos.x;
            right = pos.x;
            top = pos.y;
            bottom = pos.y;
        }

        internal void UpdateWithPosition(Vector2 pos)
        {
            if (pos.x < left)
                left = pos.x;
            else if (pos.x > right)
                right = pos.x;

            if (pos.y < bottom)
                bottom = pos.y;
            else if (pos.y > top)
                top = pos.y;
        }

        internal void Reset()
        {
            bottom = 0;
            top = 0;
            left = 0;
            right = 0;
            
        }
    }
}