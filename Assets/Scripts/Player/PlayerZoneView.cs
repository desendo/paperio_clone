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
        [Inject]
        GameSettingsInstaller.DebugSettings debugSettings;
        MeshFilter _meshFilter;
        MeshRenderer _meshRenderer;
        public GameObject PlayerZoneViewContainer { get; private set; }

        public PlayerZoneView(PlayerZone playerZone)
        {
            _playerZone = playerZone;
        }
        internal void UpdateMesh()
        {           

            int[] indices = Triangulator.Triangulate(_playerZone.BorderPoints);

            Vector3[] vertices = new Vector3[_playerZone.BorderPoints.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(_playerZone.BorderPoints[i].x, _playerZone.BorderPoints[i].y, 0);

               
            }
            Mesh _mesh = new Mesh();

            _mesh.vertices = vertices;
            _mesh.triangles = indices;
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _meshFilter.mesh = _mesh;           

        }
        
        public void Initialize()
        {
            PlayerZoneViewContainer = new GameObject("PlayerZoneViewContainer");
            PlayerZoneViewContainer.transform.parent = _playerFacade.transform;
            PlayerZoneViewContainer.transform.position = new Vector3(0, 0, _settings.height);
            _meshRenderer = PlayerZoneViewContainer.AddComponent<MeshRenderer>();
            _meshFilter = PlayerZoneViewContainer.AddComponent<MeshFilter>();
            _meshRenderer.material = _settings.zoneMaterial;
            _meshRenderer.material.color = _playerFacade.MainColor;
        }

        [System.Serializable]
        public class Settings
        {
            public Material zoneMaterial;
            public float height;

        }
    }
}