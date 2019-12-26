using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerLine :ITickable,IInitializable
    {
        //todo выделить view class
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
        public List<Vector2> LineDots
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
        public int InsertPointToLastSegment(Vector2 crossing)
        {
            int index = 0;
            if (_lineDots.Count > 0)
            {
                _lineDots.Insert(_lineDots.Count - 1, crossing);
                index = _lineDots.Count - 1;
            }

            return index;
        }


        public void AddDot(Vector3 pos)
        {

           // Helpers.PlaceCube(pos, Color.red);
            _lineDots.Add(pos);
            if(_lineDots.Count>1)
                lineCrossingController.OnLinePointAdded(this);
            if(_lineDots.Count==1)
                lineRect.InitWithPosition(pos);
            else
                lineRect.UpdateWithPosition(pos);
        }        
        public void ClearLine()
        {
            _lineDots.Clear();
            lineRect.Reset();
        }


        public void Tick()
        {
            Debug.DrawLine(new Vector3(lineRect.left, lineRect.bottom), new Vector3(lineRect.right, lineRect.top),Color.red);


            lineRenderer.positionCount = LineDots.Count;
            Vector3[] lineDotsArray = new Vector3[LineDots.Count];
            for (int i = 0; i < lineDotsArray.Length; i++)
            {
                lineDotsArray[i] = new Vector3(LineDots[i].x, LineDots[i].y, _settings.height);
            }
            lineRenderer.SetPositions(lineDotsArray);
        }


        
        public void Initialize()
        {
            lineRendererContainer = new GameObject("Line View Container");
            lineRendererContainer.transform.parent = Facade.transform;
            lineRenderer = lineRendererContainer.AddComponent<LineRenderer>();
            lineRenderer.material = _settings.lineMaterial;
            
            lineRenderer.material.color = Facade.MainColor; 
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