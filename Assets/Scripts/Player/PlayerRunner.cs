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
        PlayerZone homeZone;
        [Inject]
        PlayerZoneView ZoneView;
        [Inject]
        PlayerZoneService ZoneService;

        [Inject]
        PlayersRegistry playersRegistry;
        Rigidbody2D _rigidBody;        
        Renderer[] _renderers;
        Transform _transform;


        float squaredDeltaPos;
        Vector3 oldPosition;

        bool wasOutsideHomeZone;

        Vector3 exitPoint;
        int exitPointIndex;
        Vector3 entryPoint;
        int enterPointIndex;
        public void Tick()
        {

            CheckZonesCrossings();

            var isOutsideHomeZone = IsOutsideHomeZome;

            if (isOutsideHomeZone)
                Line.DrawLine();

            foreach (var item in playersRegistry.Zones)
            {
              //  Debug.Log(playersRegistry.Zones.Count);
            }

            if (isOutsideHomeZone && wasOutsideHomeZone != isOutsideHomeZone)
            {
                HandleHomeZoneExit();
            }
            if (!isOutsideHomeZone && wasOutsideHomeZone != isOutsideHomeZone)
            {
                HandleHomeZoneEnter();

            }
            wasOutsideHomeZone = isOutsideHomeZone;

            if (isOutsideHomeZone)
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

        private void CheckZonesCrossings()
        {
            foreach (var zoneToCheck in playersRegistry.Zones)
            {

                if (zoneToCheck.IsInZone(Position))
                {

                }
            }
        }

        void HandleHomeZoneExit()
        {
            Debug.Log("exit zone");
            SignalsController.Default.Send(
            new SignalZoneBorderPass()
            {
                playerRunner = this,
                isExiting = true,
                zone = homeZone,
                nearestBorderPointIndex = homeZone.GetNearestBorderPointTo(Position)
            });
        }

        void HandleHomeZoneEnter()
        {

            Debug.Log("enter zone");
            SignalsController.Default.Send(
            new SignalZoneBorderPass()
            {
                playerRunner = this,
                isExiting = false,
                zone = homeZone,
                nearestBorderPointIndex = homeZone.GetNearestBorderPointTo(Position)
            });

            ZoneView.UpdateMesh();
            Line.ClearLine();
        }
        public void Initialize()
        {
            _transform = transform;
            _renderers = _transform.GetComponentsInChildren<Renderer>();
            _rigidBody = GetComponent<Rigidbody2D>();
            oldPosition = _transform.position;
            
            
          //  ZoneView.UpdateMesh();

        }

        public Renderer[] Renderers
        {
            get { return _renderers; }
        }
        public Vector3 LookDir
        {
            get { return _rigidBody.transform.right; }
        }
        public float Rotation
        {
            get { return _transform.rotation.eulerAngles.z; }
            set { _transform.eulerAngles = new Vector3(_transform.rotation.eulerAngles.x, _transform.rotation.eulerAngles.y,value); }
        }
        public bool IsOutsideHomeZome
        {
            get
            {
                return !Helpers.CheckIfInPolygon(homeZone.BorderPointsArray, _transform.position);
            }
        }
        public Vector3 Position
        {
            get { return _transform.position; }
            set { _transform.position = value; }
        }




    }
}