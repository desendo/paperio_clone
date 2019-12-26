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
        private readonly List<CrossingController.Crossing> crossings;

        private readonly List<Vector2> _lineDots;
        private float squaredDeltaPos;
        private Vector3 oldPosition;
        private GameObject lineRendererContainer;
        private LineRenderer lineRenderer;

        
        public PlayerLine(PlayerRunner playerRunner)
        {

            _playerRunner = playerRunner;
            lineRect = new Rect();
            crossings = new List<CrossingController.Crossing>();
            _lineDots = new List<Vector2>();
        }
        public List<CrossingController.Crossing> Crossings
        {
            get => crossings;
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
        public bool LineDrawEnabled { get; set; }

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


        void AddDot(Vector3 pos)
        {            
            _lineDots.Add(pos);
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
            crossings.Clear();
        }
        void CreateLine()
        {
            Vector3 position = _playerRunner.Position ;
            
            squaredDeltaPos += (position - oldPosition).sqrMagnitude * Time.deltaTime;
            if (squaredDeltaPos > _settings.squaredUnitsPerDotPerFrame)
            {
                AddDot(position);

                squaredDeltaPos = 0;
            }
            oldPosition = position;
        }
        public void Tick()
        {
            Debug.DrawLine(new Vector3(lineRect.left, lineRect.bottom), new Vector3(lineRect.right, lineRect.top),Color.red);

            if(LineDrawEnabled)
                CreateLine();

            lineRenderer.positionCount = LineDots.Count;
            Vector3[] lineDotsArray = new Vector3[LineDots.Count];
            for (int i = 0; i < lineDotsArray.Length; i++)
            {
                lineDotsArray[i] = new Vector3(LineDots[i].x, LineDots[i].y, _settings.height);
            }
            lineRenderer.SetPositions(lineDotsArray);
        }

        internal void AddCrossing(PlayerZone zone, int crossIndex, bool isEntry)
        {
            CrossingController.Crossing crossing = null;
            foreach (var item in crossings)
            {
                if (item.performed) continue;
                if (item.zone == zone && !item.passed)
                {
                    crossing = item;
                }
            }
            if (crossing != null)
            {
                if (isEntry && crossing.passed)
                    crossings.Remove(crossing);
                else
                {
                    crossing.exitIndex = crossIndex;
                    crossing.passed = true;
                }
            }
            else
            {
                if (isEntry)
                {
                    crossing = new CrossingController.Crossing();
                    crossing.enterIndex = crossIndex;
                    crossing.zone = zone;
                    Crossings.Add(crossing);
                }
                else
                    return;
                
            }
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
            public float squaredUnitsPerDotPerFrame;
            public float destroySpeed;

            public Material lineMaterial;
            public float width;
            public float height;
        }
    }
}