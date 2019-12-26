﻿using System;
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
        
        readonly PlayerRunner _playerRunner;        
        
        private List<Vector2> _lineDots = new List<Vector2>() ;
        private float squaredDeltaPos;
        private Vector3 oldPosition;
        GameObject lineRendererContainer;
        LineRenderer lineRenderer;
        Rect lineRect;

        public PlayerLine(PlayerRunner playerRunner)
        {

            _playerRunner = playerRunner;
            lineRect = new Rect();
        }
        public Rect rect
        {
            get => lineRect;
        }
        public List<Vector2> LineDots
        {
            get => _lineDots;            
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

        public Vector2 LastPoint
        {
            get => _lineDots[_lineDots.Count-1];
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
            get => new Vector2[2] { PreLastPoint, LastPoint};
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
            lineRect = new Rect();
        }
        public bool LineDrawEnabled { get; set; }
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

            //line view
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
            public float squaredUnitsPerDotPerFrame;
            public float destroySpeed;

            public Material lineMaterial;
            public float width;
            public float height;
        }
    }
}