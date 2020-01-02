using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PaperIOClone.Helpers
{
    public static class Geometry
    {
        public static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
        {
            var vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
            var sign = Vector2.Dot(vec1Rotated90, vec2) < 0 ? -1.0f : 1.0f;
            return Vector2.Angle(vec1, vec2) * sign;
        }

        public static Vector3 TrimPositionToWorldBounds(Vector3 position, float radius, Vector2 center)
        {
            var distToWorldCenter = (Vector2) position - center;

            var deltaMag = distToWorldCenter.magnitude;
            if (deltaMag < radius) return position;

            var dir = distToWorldCenter.normalized;
            var positionUpdated = dir * radius + center;
            return new Vector3(positionUpdated.x, positionUpdated.y, position.z);
        }

        public static void PlaceDebugLine(List<Vector2> line, Color color, GameObject prefab, string contextName,
            string prefix, float expanse)
        {
            var median = line[0];
            for (var i = 1; i < line.Count; i++) median += line[i];

            var context = GameObject.Find(contextName);
            if (context == null)
                context = new GameObject(contextName);
            foreach (Transform item in context.transform) Object.Destroy(item.gameObject);
            var parent = context.transform;
            for (var i = 0; i < line.Count; i++)
            {
                var shift = (line[i] - median).normalized;
                var go = PlaceDebugCube(line[i] + shift * expanse, color, prefab, prefix + " " + i);
                go.transform.parent = parent;
            }
        }

        public static GameObject PlaceDebugCube(Vector3 pos, Color color, GameObject prefab = null, string text = "")
        {
            GameObject t;
            if (prefab == null)
                t = GameObject.CreatePrimitive(PrimitiveType.Cube);
            else
                t = Object.Instantiate(prefab);
            t.transform.position = pos;
            t.transform.localScale *= 0.3f;
            t.transform.GetComponentInChildren<MeshRenderer>().material.color = color;
            if (!string.IsNullOrEmpty(text))
            {
                var tmpro = t.transform.GetComponentInChildren<TMP_Text>();
                tmpro.text = text;
                t.name = text;
            }

            return t;
        }

        public static bool CheckIfInPolygon(List<Vector2> points, Vector2 point, bool includeBorders = false)
        {
            int i, j, nvert = points.Count;
            var c = false;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
                if (includeBorders)
                {
                    if (points[i].y >= point.y != points[j].y >= point.y &&
                        point.x <= (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) +
                        points[i].x
                    )
                        c = !c;
                }
                else
                {
                    if (points[i].y > point.y != points[j].y > point.y &&
                        point.x < (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) +
                        points[i].x)
                        c = !c;
                }

            return c;
        }

        public static Color GetRandomColor()
        {
            var random = new Random();
            return new Color(
                Random.value,
                Random.value,
                Random.value);
        }

        internal static int GetNearestBorderPointTo(List<Vector2> borderPoints, Vector3 position)
        {
            var sqaredDist = float.PositiveInfinity;
            var indexOfNearest = -1;
            for (var i = 0; i < borderPoints.Count; i++)
            {
                var x1 = borderPoints[i].x;
                var y1 = borderPoints[i].y;

                var x2 = position.x;
                var y2 = position.y;
                var sqaredDistMin = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                if (sqaredDistMin < sqaredDist)
                {
                    indexOfNearest = i;
                    sqaredDist = sqaredDistMin;
                }
            }

            return indexOfNearest;
        }

        public static bool SegmentCrossesPolyline(
            Vector2 point1, Vector2 point2,
            List<Vector2> polyline,
            out Vector2 crossing, List<int> polylineIndexPair, bool loop = true)
        {
            crossing = Vector2.zero;
            if (polyline == null || polyline.Count < 2) return false;
            for (var i = 0; i < polyline.Count; i++)
            {
                var second = i + 1;
                if (i + 1 == polyline.Count && loop)
                    second = 0;
                else if(i + 1 == polyline.Count)
                    continue;
                if (CheckIfTwoSegmentsIntersects(point1, point2, polyline[i], polyline[second], out crossing))
                {
                    if (polylineIndexPair != null)
                    {
                        var i2 = i + 1;
                        if (i2 >= polyline.Count)
                            i2 = 0;
                        polylineIndexPair.Add(i);
                        polylineIndexPair.Add(i2);
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool SegmentCrossesPolyline(Vector2[] segment, List<Vector2> polyline, out Vector2 crossing, bool loop)
        {
            return SegmentCrossesPolyline(segment[0], segment[1], polyline, out crossing, null, loop);
        }

        public static bool SegmentCrossesPolyline(Vector2[] segment, List<Vector2> polyline, out Vector2 crossing,
            List<int> polylineIndexPair)
        {
            return SegmentCrossesPolyline(segment[0], segment[1], polyline, out crossing, polylineIndexPair);
        }

        public static void SimplifyPolyline(List<Vector2> borderPoints, float distanceSimplifiy)
        {
            var borderArray = borderPoints.ToArray();
            var borderPointsUpdated = new List<Vector2>();
            for (var i = 0; i < borderArray.Length; i++)
            {
                var i2 = i + 1;
                if (i2 == borderArray.Length) i2 = 0;
                var delta = (borderArray[i] - borderArray[i2]).sqrMagnitude;
                if (delta > distanceSimplifiy) borderPointsUpdated.Add(borderArray[i]);
            }

            borderPoints = borderPointsUpdated;
        }

        public static bool CheckIfTwoSegmentsIntersects(Vector2 line1Point1, Vector2 line1Point2, Vector2 line2Point1,
            Vector2 line2Point2, out Vector2 crossing)
        {
            crossing = Vector2.zero;
            var cut1 = line1Point2 - line1Point1;
            var cut2 = line2Point2 - line2Point1;

            var prod1 = 0f;
            var prod2 = 0f;

            if (line1Point1 == line2Point1 ||
                line1Point1 == line2Point2 ||
                line1Point2 == line2Point1 ||
                line1Point2 == line2Point2)
                return false;

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

        internal static List<Vector2> GenerateCirclePolygon(float initialRadius, int initialDotsCount,
            Vector2 position2D)
        {
            var border = new List<Vector2>();
            var vertsCount = initialDotsCount;
            var r = initialRadius;
            var step = 360f / vertsCount;
            var phase = Random.value * 360f * 3.14159265359f;
            //float phase = 0;
            for (var i = 0; i < vertsCount; i++)
            {
                var rad = i * step / 180.0f * 3.14159265359f + phase;
                var x = r * Mathf.Cos(rad);
                var y = r * Mathf.Sin(rad);
                border.Add(new Vector2(x, y) + position2D);
            }

            return border;
        }
    }
}