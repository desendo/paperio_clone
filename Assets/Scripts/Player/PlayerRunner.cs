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
        PlayerLine Line;
        [Inject]
        PlayerZone Zone;
        [Inject]
        PlayerZoneView ZoneView;
        [Inject]
        PlayerZoneService ZoneService;

        Rigidbody2D _rigidBody;        
        Renderer[] _renderers;
        Transform _transform;


        float squaredDeltaPos;
        Vector3 oldPosition;

        bool previousStateIsOutside;

        Vector3 exitPoint;
        int exitPointIndex;
        Vector3 entryPoint;
        int enterPointIndex;
        public void Tick()
        {

            var isOutside = IsOutside;

            if (isOutside)
                Line.DrawLine();

            if (isOutside && previousStateIsOutside != isOutside)
            {
                HandleHomeZoneExit();
            }
            if (!isOutside && previousStateIsOutside != isOutside)
            {
                HandleHomeZoneEnter();

            }

            previousStateIsOutside = isOutside;
            if (isOutside)
            {
                foreach (var item in _renderers)
                {
                    item.material.color = Color.red;
                }
            }
            else
            {
                foreach (var item in _renderers)
                {
                    item.material.color = Color.green;
                }
            }
        }
        void HandleHomeZoneExit()
        {
            ZoneService.ExitHomeZone(_transform.position);

        }

        void HandleHomeZoneEnter()
        {
            ZoneService.EnterHomeZone(_transform.position);
            ZoneView.UpdateMesh();
            Line.ClearLine();
        }
        public void Initialize()
        {
            _transform = transform;
            _renderers = _transform.GetComponentsInChildren<Renderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            oldPosition = _transform.position;
            
            
            Zone.Initialize();
            Zone.GenerateCirclePolygon();
            ZoneView.UpdateMesh();

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
            get
            {
                return !Helpers.CheckIfInPolygon(Zone.BorderPointsArray, _transform.position);
            }
        }        


        public Vector2 Position
        {
             get { return _transform.position; }
             set { _transform.position = value; }
        }

    }
}