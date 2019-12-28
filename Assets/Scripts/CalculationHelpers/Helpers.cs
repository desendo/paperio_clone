using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{

    static class Extensions
    {

        public static T CircleNext<T>(this List<T> list, int index)
        {            
            if (index == list.Count - 1)
                return list[0];
            else 
                return list[index + 1];
        }

        public static T CirclePrev<T>(this List<T> list, int index)
        {
            if (index == 0)
                return list[list.Count - 1];
            else
                return list[index - 1];
        }

    }


    public static class Helpers 
    {

        public static void PlaceDebugLine(List<Vector2> line, Color color, GameObject prefab, string contextName, string prefix, float expanse)
        {

            Vector2 median = line[0];
            for (int i = 1; i < line.Count; i++)
            {
                median += line[i];
            }

            var context = GameObject.Find(contextName);
            if (context == null)
                context = new GameObject(contextName);
            foreach (Transform item in context.transform)
            {
                GameObject.Destroy(item.gameObject);
            }
            Transform par = context.transform;
            for (int i = 0; i < line.Count; i++)
            {
                
                Vector2 shift = (line[i] - median).normalized;
                var go = PlaceDebugCube(line[i] + shift*expanse, color, prefab, prefix+" "+i.ToString());
                go.transform.parent = par;
            }
        }
        public static GameObject PlaceDebugCube(Vector3 pos, Color color, GameObject prefab = null, string text= "")
        {
            GameObject t;
            if (prefab == null)
                t = GameObject.CreatePrimitive(PrimitiveType.Cube);
            else
                t = GameObject.Instantiate(prefab);
            t.transform.position = pos;
            t.transform.localScale *= 0.3f;
            t.transform.GetComponentInChildren<MeshRenderer>().material.color = color;
            if (!string.IsNullOrEmpty(text))
            {
                var tmpro = t.transform.GetComponentInChildren<TMPro.TMP_Text>();
                tmpro.text = text;
                t.name = text;

            }

            return t;
        }
        public static bool CheckIfInPolygon(List<Vector2> points, Vector2 point, bool includeBorders = false)
        {

            
            int i, j, nvert = points.Count;
            bool c = false;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (includeBorders)
                {
                    if (((points[i].y >= point.y) != (points[j].y >= point.y)) &&
                    (point.x <= (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) + points[i].x)
                  )
                        c = !c;
                }
                else
                {
                    if (((points[i].y > point.y) != (points[j].y > point.y)) &&
                 (point.x < (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                        c = !c;
                }
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

        public static bool SegmentCrossesPolyline(
            Vector2 Point1, Vector2 Point2,
            List<Vector2> polyline, 
            ref Vector2 crossing, List<int> polylineIndexPair)
        {
            crossing = Vector2.zero;
            if (polyline == null || polyline.Count < 2) return false;
            else
            {
                for (int i = 0; i < polyline.Count; i++)
                {
                    int second = i + 1;
                    if (i + 1 == polyline.Count)
                        second = 0;
                    if (CheckIfTwoSegmentsIntersects(Point1, Point2, polyline[i], polyline[second], ref crossing))
                    {
                        if (polylineIndexPair != null)
                        {
                            int i2 = i + 1;
                            if (i2 >= polyline.Count)
                                i2 = 0;
                            polylineIndexPair.Add(i);
                            polylineIndexPair.Add(i2);
                        }
                        return true;
                    }
                }
            }

            return false;
        }
        public static bool SegmentCrossesPolyline(Vector2[] segment, List<Vector2> polyline, ref Vector2 crossing)
        {
            return SegmentCrossesPolyline(segment[0], segment[1], polyline, ref crossing, null);
        }

        public static bool SegmentCrossesPolyline(Vector2[] segment, List<Vector2> polyline, ref Vector2 crossing, List<int> polylineIndexPair)
        {
            return SegmentCrossesPolyline(segment[0], segment[1], polyline, ref crossing, polylineIndexPair);
        }

        public static void SimplifyPolyline(List<Vector2> borderPoints, float distanceSimplifiy)
        {

            var borderArray = borderPoints.ToArray();
            List<Vector2> borderPointsUpdated= new List<Vector2>(); 
            for (int i = 0; i < borderArray.Length; i++)
            {
                int i2 = i + 1;
                if (i2 == borderArray.Length) i2 = 0;
                var delta = (borderArray[i] - borderArray[i2]).sqrMagnitude;
                if (delta > distanceSimplifiy)
                {

                    borderPointsUpdated.Add(borderArray[i]);
                }
            }
            borderPoints = borderPointsUpdated;
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
            //float phase = 0;
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