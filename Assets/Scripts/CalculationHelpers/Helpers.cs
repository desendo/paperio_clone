using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public static class Helpers 
    {
        public static bool CheckIfInPolygon(Vector2[] polygon, Vector2 point)
        {

            float x = point.x;
            float y = point.y;
            bool c = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                
                if ((
                    (polygon[i].y < polygon[j].y) && (polygon[i].y <= y) && (y <= polygon[j].y) &&
                    ((polygon[j].y - polygon[i].y) * (x - polygon[i].x) > (polygon[j].x - polygon[i].x) * (y - polygon[i].y))
                ) || (
                    (polygon[i].y > polygon[j].y) && (polygon[j].y <= y) && (y <= polygon[i].y) &&
                    ((polygon[j].y - polygon[i].y) * (x - polygon[i].x) < (polygon[j].x - polygon[i].x) * (y - polygon[i].y))
                ))
                    c = !c;
            }
            return c;
  
        }

    }
}