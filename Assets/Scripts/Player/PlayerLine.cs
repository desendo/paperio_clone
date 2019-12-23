using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerLine 
    {
        readonly PlayerFacade _playerFacade;

        private List<Vector2> _lineDots;

        public PlayerLine(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
            _lineDots = new List<Vector2>() ;
        }

        public IEnumerable<Vector2> LineDots
        {
            get => _lineDots;            
        }

        [System.Serializable]
        public class Settings
        {
            public float discretizationFactor;
            public float destroySpeed;
        }

        internal void AddDot(Vector3 pos)
        {
            _lineDots.Add(pos);
            Debug.Log("adddot");
        }
    }
}