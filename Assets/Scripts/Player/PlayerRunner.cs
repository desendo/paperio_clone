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
        PlayerFacade Facade;
        [Inject]
        GameSettingsInstaller.DebugSettings debugSettings;
        [Inject]
        Settings _settings;
        float squaredDeltaPos;

        bool wasOutsideHomeZone;

        Vector3 exitPoint;
        Vector3 position;
        Vector3 lookDir;
        float rotation;
        int exitPointIndex;
        Vector3 entryPoint;
        int enterPointIndex;
        public Vector2 LastInsideHome { get; private set; }

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

        public void SetCrown(bool isOn)
        {
            view.SetCrown(isOn);
        }

        public void Initialize()
        {

            oldPosition = Position;
            oldInside = true;
            LastInsideHome = Position;
            
        }

        public void OnSpawn()
        {
            oldPosition = Position;
            oldInside = true;
            LastInsideHome = Position;
            LookDir = Vector2.right;
            Rotation = 0;
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
                CheckIfZoneCrossing();
                CheckIfLineCrossing();
                oldPosition = Position;
            }
        }

        private void CheckIfLineCrossing()
        {
            crossingController.CheckLineCrossings(oldPosition, Position, Facade);
        }
        private void CheckIfZoneCrossing()
        {
            bool isInside = Inside;//кешируем 
            List<int> borderCrossingEdgeIndexes = new List<int>();

            bool isCrossing = Helpers.SegmentCrossesPolyline(Position, oldPosition, zone.BorderPoints, out Vector2 crossPoint, borderCrossingEdgeIndexes);

            if (oldInside != isInside && !isCrossing)
            {

                Debug.LogError( Facade.Name +" переход без фиксации пересечения. возможно ошибка построения меша");
                Debug.LogError( Facade.Name +" внутри: "+Inside);
                Helpers.PlaceDebugLine(zone.BorderPoints, Helpers.GetRandomColor(), debugSettings.digitCubePrefab, Facade.Name + " dd", "", 0f);
                Debug.Break();
            }

            if (oldInside != isInside && isCrossing)
            {

                if (!isInside)
                {
                    LastInsideHome = oldPosition;
                }

                List<Vector2> updatedborder = new List<Vector2>();
                oldInside = isInside;
                for (int i = 0; i < zone.BorderPoints.Count; i++)
                {
                    updatedborder.Add(zone.BorderPoints[i]);
                    if (borderCrossingEdgeIndexes.Count > 1 && i == borderCrossingEdgeIndexes[0])
                    {
                        updatedborder.Add(crossPoint);
                    }
                }
                zone.SetBorder(updatedborder);

                line.AddDot(crossPoint);
                if (isInside)
                {
                    HandleHomeZoneEnter();
                }
                else
                {
                    //HandleHomeZoneExit();
                }
            }
            else if (!isInside)
            {
                line.AddDot(Position);
            }
        }

        void HandleHomeZoneEnter()
        {
            ZoneService.HandleEnterHomeZone();
            zoneView.UpdateMesh();
            line.ClearLine();
            crossingController.PerformCuts(zone);
            
        }
        public Vector3 LookDir
        {
            get { return view.LookDir; }
            set{view.LookDir = value;}
        }
        public float Rotation
        {
            get { return rotation; }
            set
            {
                view.Rotation = value;
                rotation = value;
            }
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
            get { return position; }
            set
            {
                position = value;
                view.Position = value;
            }
        }
        [System.Serializable]
        public class Settings
        {
            public float unitPerPoint;
        }

    }
}