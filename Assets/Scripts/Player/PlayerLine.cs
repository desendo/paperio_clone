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
        LineCrossingController lineCrossingController;

        [Inject]
        public PlayerFacade playerFacade { get; set; }

        
        readonly PlayerRunner _playerRunner;
        
        
        private List<Vector3> _lineDots = new List<Vector3>() ;
        private float squaredDeltaPos;
        private Vector3 oldPosition;
        GameObject lineRendererContainer;
        LineRenderer lineRenderer;
        public PlayerLine(PlayerRunner playerRunner)
        {

            _playerRunner = playerRunner;
            
        }

        public List<Vector3> LineDots
        {
            get => _lineDots;            
        }
        
        void AddDot(Vector3 pos)
        {            
            _lineDots.Add(pos);
            lineCrossingController.DotAdded(this);

        }
        public void ClearLine()
        {
            _lineDots.Clear();
        }
        public void CreateLine()
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
           // if (LineDrawEnabled)
             //   CreateLine();
            //lineRenderer.positionCount = LineDots.Count+1;
            lineRenderer.positionCount = LineDots.Count;
            lineRenderer.SetPositions(LineDots.ToArray());
           // lineRenderer.SetPosition(LineDots.Count,_playerRunner.Position);            

        }

  

        public void Initialize()
        {
            lineRendererContainer = new GameObject("lineRendererContainer");
            lineRendererContainer.transform.parent = playerFacade.transform;
            lineRenderer = lineRendererContainer.AddComponent<LineRenderer>();
            lineRenderer.material = _settings.lineMaterial;
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
        }
    }
}