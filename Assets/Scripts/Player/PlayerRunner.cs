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
                Vector2 pointPosition = oldPosition;
                bool isCrossingHomeZone = Helpers.SegmentCrossesPolyline(oldPosition, Position, zone.BorderPoints, ref pointPosition,false);
                
                if (isCrossingHomeZone)
                {
                    Helpers.InsertPointOnCrossing(zone.BorderPoints, pointPosition);
                    if (IsOutsideHomeZome)
                    {                        
                        line.AddDot(pointPosition);                        
                        HandleHomeZoneExit(pointPosition);
                    }
                    else
                    {                        
                        line.AddDot(pointPosition);                        
                        HandleHomeZoneEnter(pointPosition);
                    }
                }
                else if (IsOutsideHomeZome)
                {
                    line.AddDot(Position);
                }
                
                oldPosition = Position;                    
            }

        }

        void HandleHomeZoneExit(Vector2 crossingPosition)
        {
            SignalsController.Default.Send(
            new SignalZoneBorderPass()
            {
                playerRunner = this,
                isExiting = true,
                zone = zone,
                nearestBorderPointIndex = zone.GetNearestBorderPointTo(crossingPosition)
            });
            
        }

        void HandleHomeZoneEnter(Vector2 crossingPosition)
        {
            
            SignalsController.Default.Send(
            new SignalZoneBorderPass()
            {
                playerRunner = this,
                isExiting = false,
                zone = zone,
                nearestBorderPointIndex = zone.GetNearestBorderPointTo(crossingPosition)
            });

            zoneView.UpdateMesh();
            line.ClearLine();
            crossingController.PerformCuts(zone);
            
        }

        public void Initialize()
        {
            oldPosition = Position;

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