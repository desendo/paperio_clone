﻿using System.Collections;
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
        GameSettingsInstaller.DebugSettings debugSettings;
        [Inject]
        Settings _settings;
        float squaredDeltaPos;

        bool wasOutsideHomeZone;

        Vector3 exitPoint;
        int exitPointIndex;
        Vector3 entryPoint;
        int enterPointIndex;


        Vector3 oldPosition;
        bool oldInside;
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


        public void Initialize()
        {
            oldPosition = Position;
            oldInside = true;

        }
        public void Tick()
        {
            CheckHomeZoneCrossings();

        }

        private void CheckHomeZoneCrossings()
        {
            float deltaPosition = (Position - oldPosition).magnitude;

            if (deltaPosition > _settings.unitPerPoint)
            {

                Vector2 crossPoint = new Vector2();
                List<int> p = new List<int>();

                bool isCrossing = Helpers.SegmentCrossesPolyline(Position, oldPosition, zone.BorderPoints, ref crossPoint, p);

                
                if (oldInside != Inside && isCrossing)
                {

                    List<Vector2> updatedborder = new List<Vector2>();
                    oldInside = Inside;
                    for (int i = 0; i < zone.BorderPoints.Count; i++)
                    {
                        updatedborder.Add(zone.BorderPoints[i]);
                        if (i == p[0])
                            updatedborder.Add(crossPoint);
                    }
                    zone.SetBorder( updatedborder);                   
                   
                    if (Inside)
                    {
                        HandleHomeZoneEnter();
                    }                    
                    line.AddDot(crossPoint);
                }
                else if (!Inside)
                {
                    line.AddDot(Position);

                }
                oldPosition = Position;

            }
        }

        void HandleHomeZoneEnter()
        {

            ZoneService.HandleEnterHomeZone();
            zoneView.UpdateMesh();
            line.ClearLine();
            //crossingController.PerformCuts(zone);
            
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
        public bool Inside
        {
            get
            {                
                return Helpers.CheckIfInPolygon(zone.BorderPoints, Position);
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