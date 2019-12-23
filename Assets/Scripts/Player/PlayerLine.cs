using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerLine 
    {
        [Inject]
        Settings _settings = null;
        [Inject]
        PlayerRunner _playerRunner = null;
        readonly PlayerFacade _playerFacade;

        private List<Vector2> _lineDots;
        private float squaredDeltaPos;
        private Vector3 oldPosition;
        public PlayerLine(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
            _lineDots = new List<Vector2>() ;
        }

        public IEnumerable<Vector2> LineDots
        {
            get => _lineDots;            
        }

        void AddDot(Vector3 pos)
        {
            _lineDots.Add(pos);
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

        [System.Serializable]
        public class Settings
        {
            public float squaredUnitsPerDotPerFrame;
            public float destroySpeed;
        }
    }
}