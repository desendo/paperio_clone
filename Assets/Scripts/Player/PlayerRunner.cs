using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

namespace Game
{
    public class PlayerRunner : MonoBehaviour, ITickable,IInitializable
    {
        [Inject]
        Settings _settings;
        Rigidbody2D _rigidBody;        
        Renderer[] _renderers;
        Transform _transform;


        float squaredDeltaPos;
        Vector3 oldPosition;
        public void Tick()
        {

            squaredDeltaPos += (_transform.position - oldPosition).sqrMagnitude * Time.deltaTime * Time.deltaTime * 3600f;
            if (squaredDeltaPos > _settings.squaredDeltaPositionPerDot)
            {
                PlaceDot(_transform.position);
                squaredDeltaPos = 0;
                
            }
            oldPosition = _transform.position;
        }

        private void PlaceDot(Vector3 pos)
        {
            Line.AddDot(pos);
        }

        public void Initialize()
        {
            _transform = transform;
            _renderers = _transform.GetComponentsInChildren<Renderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            oldPosition = _transform.position;

            Debug.Log("runner Initialize");
            Zone.Initialize();
            Zone.GenerateCirclePolygon();
            Zone.UpdateMesh();
        }

        [Inject]
        public PlayerFacade Facade
        {
            get; set;
        }
        [Inject]
        public PlayerLine Line
        {
            get; set;
        }
        [Inject]
        public PlayerZone Zone
        {
            get; set;
        }
        public Renderer[] Renderers
        {
            get { return _renderers; }
        }
        public Vector2 LookDir
        {
            get { return _rigidBody.transform.right; }
        }

        public float Rotation
        {
            get { return _transform.rotation.eulerAngles.z; }
            set { _transform.eulerAngles = new Vector3(_transform.rotation.eulerAngles.x, _transform.rotation.eulerAngles.y,value); }
        }
        public bool IsOutside
        {
            get; set;
        }
        public bool IsCutOf
        {
            get; set;
        }
        public bool IsDead
        {
            get; set;
        }
        public Vector2 Position
        {
             get { return _transform.position; }
             set { _transform.position = value; }
        }
        [Serializable]
        public class Settings
        {
            public float squaredDeltaPositionPerDot;
        }
    }
}