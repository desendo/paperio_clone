using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

namespace Game
{
    public class PlayerRunner :  ITickable
    {        
        
        PlayerLine line;
        PlayerRunnerView view;        
        PlayerZone zone;        
        PlayerZoneView zoneView;        
        PlayerZoneService ZoneService;
        [Inject]
        PlayersRegistry playersRegistry;


        float squaredDeltaPos;

        bool wasOutsideHomeZone;

        Vector3 exitPoint;
        int exitPointIndex;
        Vector3 entryPoint;
        int enterPointIndex;

        [Inject]
        public void Construct
        (
            PlayerLine line,
            PlayerRunnerView view,
            PlayerZone homeZone,
            PlayerZoneView ZoneView,
            PlayerZoneService ZoneService
        )
        {
            this.line = line;
            this.view = view;
            this.zone = homeZone;
            this.zoneView = ZoneView;
            this.ZoneService = ZoneService;

            Debug.Log("PlayerRunner constructor");
            var clickStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0));

            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(xs => xs.Count >= 2)
            .Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count));
        }
        public void Tick()
        {
            CheckZonesCrossings();

            var isOutsideHomeZone = IsOutsideHomeZome;

            if (isOutsideHomeZone )
            {
               line.CreateLine();
            }
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


        }
        bool _cutoff;
        internal void CutOff()
        {
            _cutoff = true;
        }

        private void CheckZonesCrossings()
        {
            foreach (var zoneToCheck in playersRegistry.Zones)
            {

           //     if (zoneToCheck.IsInZone(Position))
         
            }
        }

        void HandleHomeZoneExit()
        {
            Debug.Log("HandleHomeZoneExit");
            SignalsController.Default.Send(
            new SignalZoneBorderPass()
            {
                playerRunner = this,
                isExiting = true,
                zone = zone,
                nearestBorderPointIndex = zone.GetNearestBorderPointTo(Position)
            });
            
        }

        void HandleHomeZoneEnter()
        {
            Debug.Log("HandleHomeZoneEnter");
            SignalsController.Default.Send(
            new SignalZoneBorderPass()
            {
                playerRunner = this,
                isExiting = false,
                zone = zone,
                nearestBorderPointIndex = zone.GetNearestBorderPointTo(Position)
            });

            zoneView.UpdateMesh();
            line.ClearLine();

            //Debug.Log("площадб "+homeZone.Area());
        }

        public Vector3 LookDir
        {
            get { return view.LookDir; }
        }
        public float Rotation
        {
            get { return view.Rotation; }
            set { view.Rotation = value; }
        }
        public bool IsOutsideHomeZome
        {
            get
            {
                
                return !Helpers.CheckIfInPolygon(zone.BorderPointsList, Position);
            }
        }
        public Vector3 Position
        {
            get { return view.Position; }
            set { view.Position = value; }
        }

    }
}