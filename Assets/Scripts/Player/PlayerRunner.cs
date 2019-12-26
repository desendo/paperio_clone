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
        [Inject]
        CrossingController crossingController;

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
            PlayerZoneView zoneView,
            PlayerZoneService zoneService
        )
        {
            this.line = line;
            this.view = view;
            this.zone = homeZone;
            this.zoneView = zoneView;
            this.ZoneService = zoneService;

            var clickStream = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0));

            clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(xs => xs.Count >= 2)
            .Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count));
        }
        public void Tick()
        {

            var isOutsideHomeZone = IsOutsideHomeZome;

            if (isOutsideHomeZone )
            {
               //line.CreateLine();
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
        public void CutOff()
        {
            _cutoff = true;
        }

        private void CheckZonesCrossings()
        {

        }

        void HandleHomeZoneExit()
        {
            line.LineDrawEnabled = true;
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
            crossingController.PerformCuts(line);

            line.LineDrawEnabled = false;
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
                return !Helpers.CheckIfInPolygon(zone.BorderPoints, Position);
            }
        }
        public Vector3 Position
        {
            get { return view.Position; }
            set { view.Position = value; }
        }

    }
}