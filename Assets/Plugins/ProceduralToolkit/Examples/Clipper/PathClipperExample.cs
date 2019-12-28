using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit.ClipperLib;

namespace ProceduralToolkit.Examples
{
    public class PathClipperExample : MonoBehaviour
    {
        public LineRenderer subjectRenderer;
        public LineRenderer clipRenderer;
        public LineRenderer outputRenderer;

        private void Awake()
        {
            var subject = Geometry.StarPolygon2(5, 1, 2);

            var clip = Geometry.PointsOnCircle2(1.5f, 50);

            var output = new List<List<Vector2>>();

            var clipper = new PathClipper();
            clipper.AddPath(subject, PolyType.ptSubject);
            clipper.AddPath(clip, PolyType.ptClip);
            clipper.Clip(ClipType.ctIntersection, ref output);
        }


    }
}
