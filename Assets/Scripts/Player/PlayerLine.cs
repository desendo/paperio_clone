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
        GameSettingsInstaller.DebugSettings _debug;
        [Inject]
        Settings _settings;
        [Inject]
        PlayerRunner _playerRunner;
        [Inject]
        PlayerFacade _playerFacade;

        private List<Vector2> _lineDots;
        private float squaredDeltaPos;
        private Vector3 oldPosition;
        public PlayerLine()
        {
            
            _lineDots = new List<Vector2>() ;
        }

        public List<Vector2> LineDots
        {
            get => _lineDots;            
        }
        public Vector2[] LineDotsArray
        {
            get => _lineDots.ToArray();
        }
        private List<GameObject> dashes = new List<GameObject>();
        void AddDot(Vector3 pos)
        {
            
            _lineDots.Add(pos);
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = pos;
            go.transform.localScale *= 0.1f;
            go.GetComponent<MeshRenderer>().material = _debug.debugMaterial;
            dashes.Add(go);
            go.name = "dash " + j;
            j++;
        }
        int j = 0;
        public void ClearLine()
        {
            j = 0;
            foreach (var item in dashes)
            {
                GameObject.Destroy(item);
            }
            _lineDots.Clear();
            dashes.Clear();
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