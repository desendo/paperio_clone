using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;
namespace Game
{

    public class WorldView : MonoBehaviour
    {


        List<Vector2> border;
        MeshFilter _meshFilter;
        Mesh _mesh;
        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter = GetComponent<MeshFilter>();
            _mesh = new Mesh();

        }
        internal void UpdateView(Vector2 center, float radius, int vertexCount)
        {
            border = Helpers.GenerateCirclePolygon(radius, vertexCount, center);


            int[] indices = Triangulator.Triangulate(border);

            Vector3[] vertices = new Vector3[border.Count];
            Vector2[] uv = new Vector2[border.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(border[i].x, border[i].y, 0);
                uv[i] = new Vector2((border[i].x + radius) / (2 * radius), (border[i].y + radius) / (2 * radius));


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