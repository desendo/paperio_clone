using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public static class Helpers 
    {
        public static GameObject  PlaceCube(Vector3 pos, Color color  )
        {
            GameObject t = GameObject.CreatePrimitive(PrimitiveType.Cube);
            t.transform.position = pos;
            t.transform.localScale *= 0.3f;
            t.GetComponent<MeshRenderer>().material.color = color;

            return t;
        }
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
        
        public static Color GetRandomColor()
        {            
            UnityEngine.Random random = new UnityEngine.Random();
            return new Color(
            UnityEngine.Random.value,
            UnityEngine.Random.value,
            UnityEngine.Random.value);
        }
        
        public static bool CheckIfRectsOverlaps(Vector2 lowerLeft1, Vector2 upperRight1, Vector2 lowerLeft2, Vector2 upperRight2)
        {
            bool overlaps = false;

            return overlaps;
        }

        public static void InsertPointOnCrossing(List<Vector2> borderPoints, Vector2 pointPosition)
        {
            
        }

        internal static int GetNearestBorderPointTo(List<Vector2> borderPoints, Vector3 position)
        {
            float sqaredDist = float.PositiveInfinity;
            int indexOfNearest = -1;
            for (int i = 0; i < borderPoints.Count; i++)
            {
                float x1 = borderPoints[i].x;
                float y1 = borderPoints[i].y;

                float x2 = position.x;
                float y2 = position.y;
                float sqaredDistMin = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                if (sqaredDistMin < sqaredDist)
                {
                    indexOfNearest = i;
                    sqaredDist = sqaredDistMin;
                }
            }
            return indexOfNearest;
        }

        public static bool SegmentCrossesPolyline(Vector2 Point1, Vector2 Point2, List<Vector2> polyline, ref Vector2 crossing, bool split = false)
        {
            crossing = Vector2.zero;
            if (polyline == null || polyline.Count < 2) return false;
            else
            {

                for (int i = 0; i < polyline.Count - 1; i++)
                {
                    if (CheckIfTwoSegmentsIntersects(Point1, Point2, polyline[i], polyline[i + 1], ref crossing))
                    {
                        if (split)
                            polyline.Insert(i, crossing);
                        return true;
                    }
                }
            }

            return false;
        }
        public static bool SegmentCrossesPolyline(Vector2[] segment, List<Vector2> polyline, ref Vector2 crossing)
        {
            return SegmentCrossesPolyline(segment[0], segment[1], polyline, ref crossing);
        }
        public static bool CheckIfTwoSegmentsIntersects(Vector2 line1Point1, Vector2 line1Point2, Vector2 line2Point1, Vector2 line2Point2, ref Vector2 crossing)
        {

            crossing = Vector2.zero;
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

                crossing.x = line1Point1.x + cut1.x * Mathf.Abs(prod1) / Mathf.Abs(prod2 - prod1);
                crossing.y = line1Point1.y + cut1.y * Mathf.Abs(prod1) / Mathf.Abs(prod2 - prod1);


                return true;

            }                
        }

        internal static List<Vector2> GenerateCirclePolygon(float initialRadius, int initialDotsCount, Vector2 position2D)
        {
            List<Vector2> border = new List<Vector2>();
            int vertsCount = initialDotsCount;
            float r = initialRadius;
            float step = 360f / vertsCount;
            float phase = UnityEngine.Random.value * 360f * 3.1415f;
            for (int i = 0; i < vertsCount; i++)
            {
                float rad = (i * step) / 180.0f * 3.1415f + phase;
                float x = (r * Mathf.Cos(rad));
                float y = (r * Mathf.Sin(rad));
                border.Add(new Vector2(x, y) + position2D);
            }
            return border;
        }
    }
}