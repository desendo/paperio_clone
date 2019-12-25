using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerLine :ITickable
    {
        [Inject]
        GameSettingsInstaller.DebugSettings _debug;
        [Inject]
        Settings _settings;
        [Inject]
        LineCrossingController lineCrossingController;
        [Inject]
        public PlayerFacade PlayerFacade { get; }

        readonly PlayerRunnerView _playerRunner;
        
        private List<Vector2> _lineDots = new List<Vector2>() ;
        private float squaredDeltaPos;
        private Vector3 oldPosition;

        public List<Vector2> LineDots
        {
            get => _lineDots;            
        }
        public bool LineDrawEnabled { get; internal set; }
        
        void AddDot(Vector3 pos)
        {
            
            _lineDots.Add(pos);
            lineCrossingController.DotAdded(this);                

        }
        public void ClearLine()
        {
            _lineDots.Clear();
        }
        public void DrawLine()
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
            if (LineDrawEnabled)
                DrawLine();
        }

        [System.Serializable]
        public class Settings
        {
            public float squaredUnitsPerDotPerFrame;
            public float destroySpeed;
        }
    }
}