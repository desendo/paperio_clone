using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{

    public class PlayerZoneView 
    {
        [Inject]
        PlayerFacade _playerFacade;
        [Inject]
        Settings _settings;        
        readonly PlayerZone _playerZone;       

        MeshFilter _meshFilter;
        MeshRenderer _meshRenderer;
        GameObject PlayerZoneViewContainer;

        public PlayerZoneView(PlayerZone playerZone)
        {
            _playerZone = playerZone;
        }

        internal void UpdateMesh()
        {

            int[] indices = Triangulator.Triangulate(_playerZone.BorderPointsList);

            Vector3[] vertices = new Vector3[_playerZone.BorderPointsList.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(_playerZone.BorderPointsList[i].x, _playerZone.BorderPointsList[i].y, 0);
            }
            Mesh _mesh = new Mesh();

            _mesh.vertices = vertices;
            _mesh.triangles = indices;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();

            _meshFilter.mesh = _mesh;

            //_playerZone.DrawDebugBorder();

        }
        
        public void Initialize()
        {
            PlayerZoneViewContainer = new GameObject("PlayerZoneViewContainer");

            PlayerZoneViewContainer.transform.parent = _playerFacade.transform;
            PlayerZoneViewContainer.transform.localPosition = new Vector3(0, 0, _settings.height);
            _meshRenderer = PlayerZoneViewContainer.AddComponent<MeshRenderer>();
            _meshFilter = PlayerZoneViewContainer.AddComponent<MeshFilter>();
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