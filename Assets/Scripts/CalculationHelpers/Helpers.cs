using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public static class Helpers 
    {
        public static bool CheckIfInPolygon(List<Vector2> polygon, Vector2 point)
        {

            float x = point.x;
            float y = point.y;
            bool c = false;
            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
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

        public static bool CheckIfRectsOverlaps(Vector2 lowerLeft1, Vector2 upperRight1, Vector2 lowerLeft2, Vector2 upperRight2)
        {
            bool overlaps = false;

            return overlaps;
        }

        public static bool SegmentCrossesPolyline(Vector2 Point1, Vector2 Point2, List<Vector3> polyline)
        {
            if (polyline == null || polyline.Count < 2) return false;
            else
            {
                for (int i = 0; i < polyline.Count - 1; i++)
                {
                    if (CheckIfTwoSegmentsIntersects(Point1, Point2, polyline[i], polyline[i + 1]))
                        return true;
                }
            }
            return false;

        }

        public static bool CheckIfTwoSegmentsIntersects(Vector2 line1Point1, Vector2 line1Point2, Vector2 line2Point1, Vector2 line2Point2)
        {
            Vector2 cut1 = line1Point2 - line1Point1;
            Vector2 cut2 = line2Point2 - line2Point1;

            float prod1 = 0f;
            float prod2 = 0f;

            if (line1Point1 == line2Point1 ||
                line1Point1 == line2Point2 ||
                line1Point2 == line2Point1 ||
                line1Point2 == line2Point2)
                return false;
            else
            {
                prod1 = Vector3.Cross(cut1, line2Point1 - line1Point1).z;                    
                prod2 = Vector3.Cross(cut1, line2Point2 - line1Point1).z;

                if (prod1 < 0 == prod2 < 0)
                    return false;

                prod1 = Vector3.Cross(cut2, line1Point1 - line2Point1).z;
                prod2 = Vector3.Cross(cut2, line1Point2 - line2Point1).z;

                if (prod1 < 0 == prod2 < 0)
                    return false;

                return true;

            }

                
        }

    }
}