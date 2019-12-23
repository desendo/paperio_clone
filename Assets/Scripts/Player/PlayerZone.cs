using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerZone 
    {
        [Inject]
        Settings _settings;
        readonly PlayerFacade _playerFacade;

        private List<Vector2> _lineDots;
        private Mesh mesh;
        private MeshFilter filter;
        private GameObject meshContainer;
        private PolygonCollider2D collider2D;
        public PlayerZone(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
            _lineDots = new List<Vector2>();
            
        }
        public void Initialize()
        {
            InitComponents();
        }
        public IEnumerable<Vector2> LineDots
        {
            get => _lineDots;            
        }

        [System.Serializable]
        public class Settings
        {
            public float meshHeightOffset;
            public float initialRadius;
            public int initialDotsCount;
            public Material defaultMaterial;
        }
        void InitComponents()
        {
            meshContainer = new GameObject("zone");
            meshContainer.transform.parent = _playerFacade.transform;
            
            meshContainer.transform.localPosition =
                new Vector3(0, 0, _settings.meshHeightOffset);
            var renderer = meshContainer.AddComponent<MeshRenderer>();
            collider2D = meshContainer.AddComponent<PolygonCollider2D>();
            
            renderer.material = _settings.defaultMaterial;
            filter = meshContainer.AddComponent<MeshFilter>();
            
        }
        public void GenerateCirclePolygon()
        {
            int vertsCount = _settings.initialDotsCount;
            float r = _settings.initialRadius;
            float step = 360f / vertsCount;
            for (int i = 0; i < vertsCount; i++)
            {
                float rad = (i * step) / 180.0f * 3.14f;
                float x = (r * Mathf.Cos(rad ) );
                float y = (r * Mathf.Sin(rad));
                _lineDots.Add(new Vector2(x, y));
            }            
        }

        void UpdateCollider()
        {
            collider2D.points = _lineDots.ToArray();
        }
        public void UpdateMesh()
        {
            
            Vector2[] vertices2D = _lineDots.ToArray();
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            filter.mesh = mesh;

            UpdateCollider();

        }


    }
}