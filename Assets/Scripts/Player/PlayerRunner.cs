using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
using System;

namespace Game
{
    public class PlayerRunner :  ITickable, IInitializable
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

        [Inject]
        Settings _settings;
        float squaredDeltaPos;

        bool wasOutsideHomeZone;

        Vector3 exitPoint;
        int exitPointIndex;
        Vector3 entryPoint;
        int enterPointIndex;


        Vector3 oldPosition;
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


        }
        public void Tick()
        {
            float deltaPosition = (Position - oldPosition).magnitude;

            if (deltaPosition > _settings.unitPerPoint)
            {
                Vector2 crossing = oldPosition;
                bool isCrossingHomeZone = Helpers.SegmentCrossesPolyline(oldPosition, Position, zone.BorderPoints, ref crossing);
                
                if (isCrossingHomeZone)
                {
                    
                    
                    
                    if (IsOutsideHomeZome)
                    {
                        Debug.Log("crossing home zone exit");
                        line.AddDot(crossing);
                        HandleHomeZoneExit();
                    }
                    else
                    {
                        Debug.Log("crossing home zone enter");
                        line.AddDot(crossing);
                        HandleHomeZoneEnter();
                    }
                }
                else if(IsOutsideHomeZome)
                    line.AddDot(Position);

                
                oldPosition = Position;                    
            }

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

        public void Initialize()
        {
            oldPosition = Position;

         //   this.ObserveEveryValueChanged(x => (x.Position - x.oldPosition).magnitude).
          //      Where(delta => delta >= _settings.unitPerPoint).
        //        Subscribe(x => Debug.Log(x));

            //var clickStream = Observable.EveryUpdate().Where(_ => Position);

            //clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            //.Where(xs => xs.Count >= 2)
            //.Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count));
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
        [System.Serializable]
        public class Settings
        {
            public float unitPerPoint;
        }

    }
}