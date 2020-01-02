using System.Collections.Generic;
using PaperIOClone.Helpers;
using UnityEngine;

namespace PaperIOClone
{
    public class WorldView : MonoBehaviour
    {
        private List<Vector2> _border;
        private Mesh _mesh;
        private MeshFilter _meshFilter;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _mesh = new Mesh();
        }

        internal void UpdateView(Vector2 center, float radius, int vertexCount)
        {
            _border = Geometry.GenerateCirclePolygon(radius, vertexCount, center);

            var indices = Triangulator.Triangulate(_border);
            var vertices = new Vector3[_border.Count];
            var uv = new Vector2[_border.Count];

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(_border[i].x, _border[i].y, 0);
                uv[i] = new Vector2((_border[i].x + radius) / (2 * radius), (_border[i].y + radius) / (2 * radius));
            }

            _mesh.vertices = vertices;
            _mesh.triangles = indices;
            _mesh.uv = uv;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _meshFilter.mesh = _mesh;
        }
    }
}