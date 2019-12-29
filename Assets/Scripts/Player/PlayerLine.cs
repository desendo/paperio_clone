using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerLine :ITickable,IInitializable
    {
        [Inject]
        GameSettingsInstaller.DebugSettings _debug;
        [Inject]
        Settings _settings;
        [Inject]
        CrossingController lineCrossingController;

        [Inject]
        public PlayerFacade Facade { get; set; }

        private readonly PlayerRunner _playerRunner;
        private readonly Rect lineRect;
        private readonly List<Vector2> _lineDots;

 
        private GameObject lineRendererContainer;
        private LineRenderer lineRenderer;
        
        public PlayerLine(PlayerRunner playerRunner)
        {

            _playerRunner = playerRunner;
            lineRect = new Rect();
            _lineDots = new List<Vector2>();
        }

        public Rect rect
        {
            get => lineRect;
        }
        public List<Vector2> Points
        {
            get => _lineDots;            
        }
        public Vector2 LastPoint
        {
            get => _lineDots[_lineDots.Count - 1];
        }
        public Vector2 PreLastPoint
        {
            get
            {
                if (_lineDots.Count > 1)
                    return _lineDots[_lineDots.Count - 2];
                else
                    return Facade.Position;
            }
        }

        public Vector2[] LastSegment
        {
            get => new Vector2[2] { PreLastPoint, LastPoint };
        }
        public void AddDot(Vector3 pos)
        {
            _lineDots.Add(pos);
            
            if(_lineDots.Count==1)
                lineRect.InitWithPosition(pos);
            else
                lineRect.UpdateWithPosition(pos);
        }        
        public void ClearLine()
        {
            _lineDots.Clear();
            lineRenderer.positionCount = 0;
            lineRect.Reset();
        }


        public void Tick()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            lineRenderer.positionCount = Points.Count + 1;
            Vector3[] lineDotsArray = new Vector3[Points.Count + 1];
            for (int i = 0; i < Points.Count; i++)
            {
                lineDotsArray[i] = new Vector3(Points[i].x, Points[i].y, _settings.height);
            }
            lineDotsArray[Points.Count] = new Vector3(_playerRunner.Position.x, _playerRunner.Position.y, _settings.height);
            lineRenderer.SetPositions(lineDotsArray);
            lineRenderer.material.color = new Color(Facade.MainColor.r, Facade.MainColor.g, Facade.MainColor.b, lineRenderer.material.color.a);

        }


        public void Initialize()
        {
            lineRendererContainer = new GameObject("Line View Container");
            lineRendererContainer.transform.parent = Facade.transform;
            lineRenderer = lineRendererContainer.AddComponent<LineRenderer>();
            lineRenderer.material = _settings.lineMaterial;
            lineRenderer.startWidth = _settings.width;
            lineRenderer.endWidth = _settings.width;
        }

        [System.Serializable]
        public class Settings
        {
            public float destroySpeed;

            public Material lineMaterial;
            public float width;
            public float height;
        }
    }
}