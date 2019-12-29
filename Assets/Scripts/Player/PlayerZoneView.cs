using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{

    public class PlayerZoneView: MonoBehaviour
    {
        [Inject]
        PlayerFacade _playerFacade;
        [Inject]
        Settings _settings;
        [Inject]
        GameSettingsInstaller.DebugSettings debugSettings;
        MeshFilter _meshFilter;
        MeshRenderer _meshRenderer;
        internal void UpdateMesh()
        {           

            int[] indices = Triangulator.Triangulate(_playerFacade.Zone.BorderPoints);

            Vector3[] vertices = new Vector3[_playerFacade.Zone.BorderPoints.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(_playerFacade.Zone.BorderPoints[i].x, _playerFacade.Zone.BorderPoints[i].y, 0);

               
            }
            Mesh _mesh = new Mesh();

            _mesh.vertices = vertices;
            _mesh.triangles = indices;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _meshFilter.mesh = _mesh;

            _meshRenderer.material.color = _playerFacade.MainColor;


        }

        public void Awake()
        {
            gameObject.transform.localPosition = new Vector3(
                gameObject.transform.localPosition.x,
                gameObject.transform.localPosition.y,
                _settings.height);
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _meshFilter = gameObject.GetComponent<MeshFilter>();
            _meshRenderer.material = _settings.zoneMaterial;
        }

        [System.Serializable]
        public class Settings
        {
            public Material zoneMaterial;
            public float height;

        }
    }
}