using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Rect = PaperIOClone.Helpers.Rect;

namespace PaperIOClone.Player
{
    public class PlayerLine : ITickable, IInitializable
    {
        private LineRenderer _lineRenderer;
        private GameObject _lineRendererContainer;
        private PlayerRunner _playerRunner;
        private Settings _settings;

        public PlayerFacade Facade { get; private set; }

        public Rect Rect { get; private set; }

        public List<Vector2> Points { get; private set; }

        public void Initialize()
        {
            _lineRendererContainer = new GameObject("Line View Container");
            _lineRendererContainer.transform.parent = Facade.transform;
            _lineRenderer = _lineRendererContainer.AddComponent<LineRenderer>();
            _lineRenderer.material = _settings.lineMaterial;
            _lineRenderer.startWidth = _settings.width;
            _lineRenderer.endWidth = _settings.width;
        }

        public void Tick()
        {
            UpdateView();
        }

        [Inject]
        public void Constructor(PlayerRunner playerRunner, Settings settings, PlayerFacade playerFacade)
        {
            _playerRunner = playerRunner;
            _settings = settings;
            Facade = playerFacade;

            Rect = new Rect();
            Points = new List<Vector2>();
        }

        public void AddDot(Vector3 pos)
        {
            Points.Add(pos);

            if (Points.Count == 1)
                Rect.InitWithPosition(pos);
            else
                Rect.UpdateWithPosition(pos);
        }

        public void ClearLine()
        {
            Points.Clear();
            _lineRenderer.positionCount = 0;
            Rect.Reset();
        }

        private void UpdateView()
        {
            _lineRenderer.positionCount = Points.Count + 1;
            var lineDotsArray = new Vector3[Points.Count + 1];
            for (var i = 0; i < Points.Count; i++)
                lineDotsArray[i] = new Vector3(Points[i].x, Points[i].y, _settings.height);
            lineDotsArray[Points.Count] =
                new Vector3(_playerRunner.Position.x, _playerRunner.Position.y, _settings.height);
            _lineRenderer.SetPositions(lineDotsArray);
            var material = _lineRenderer.material;
            material.color = new Color(Facade.MainColor.r, Facade.MainColor.g, Facade.MainColor.b,
                material.color.a);
        }

        [Serializable]
        public class Settings
        {
            public float destroySpeed;
            public float height;

            public Material lineMaterial;
            public float width;
        }
    }
}