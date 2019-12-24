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

        public static float SignedPolygonArea(Vector2[] polygon)
        {
            
            // Add the first point to the end.
            float ymin = float.NegativeInfinity;
            float xmin = float.NegativeInfinity;

            for (int i = 0; i < polygon.Length; i++)
            {
                if(polygon[i].x> xmin)
                {
                    xmin = polygon[i].x;
                }
                if (polygon[i].y > ymin)
                {
                    ymin = polygon[i].y;
                }


            }
            int num_points = polygon.Length;
            Vector2[] polygon2 = new Vector2[num_points + 1];

            for (int i = 0; i < num_points; i++)
            {
                polygon2[i] = polygon[i];
            }
            polygon2[num_points] = polygon[0];
            // Get the areas.
            float area = 0;
            for (int i = 0; i < num_points; i++)
            {
                area +=
                    (polygon2[i + 1].x - polygon2[i].x ) *
                    (polygon2[i + 1].y + polygon2[i].y ) / 2;
            }

            // Return the result.
            return area;
        }
    }
}