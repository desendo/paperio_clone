﻿using System;
using PaperIOClone.Helpers;
using PaperIOClone.Installers;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Player
{
    public class PlayerZoneView : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private PlayerFacade _playerFacade;
        private Settings _settings;
        [SerializeField] private GameObject playerZoneViewContainer;

        [Inject]
        public void Constructor(PlayerFacade playerFacade, Settings settings,
            GameSettingsInstaller.DebugSettings debugSettings)
        {
            _playerFacade = playerFacade;
            _settings = settings;
        }

        internal void UpdateMesh()
        {
            var indices = Triangulator.Triangulate(_playerFacade.Zone.BorderPoints);

            var vertices = new Vector3[_playerFacade.Zone.BorderPoints.Count];
            for (var i = 0; i < vertices.Length; i++)
                vertices[i] = new Vector3(_playerFacade.Zone.BorderPoints[i].x, _playerFacade.Zone.BorderPoints[i].y,
                    0);

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            _meshFilter.mesh = mesh;

            _meshRenderer.material.color = _playerFacade.MainColor;
        }

        public void Awake()
        {
            playerZoneViewContainer.transform.localPosition = new Vector3(
                playerZoneViewContainer.transform.localPosition.x,
                playerZoneViewContainer.transform.localPosition.y,
                _settings.height);
            _meshRenderer = playerZoneViewContainer.GetComponent<MeshRenderer>();
            _meshFilter = playerZoneViewContainer.GetComponent<MeshFilter>();
            _meshRenderer.material = _settings.zoneMaterial;
        }

        [Serializable]
        public class Settings
        {
            public float height;
            public Material zoneMaterial;
        }
    }
}