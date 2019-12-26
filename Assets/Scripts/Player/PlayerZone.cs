using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerZone : IInitializable,ITickable
    {
        [Inject]
        Settings settings;
        [Inject]
        public PlayerFacade Facade { get; }
        private PlayerZoneView view;
        private PlayerZoneService service;        
        private List<Vector2> borderPoints;
        Rect zoneRect;
        [Inject]
        public void Construct(PlayerZoneView view, PlayerZoneService service)
        {
            this.view = view;
            this.service = service;
            borderPoints = new List<Vector2>();
            zoneRect = new Rect();
        }
        public float Area()
        {
            return Mathf.Abs(Triangulator.Area(BorderPoints));
        }
        public Rect rect
        {
            get => zoneRect;
        }
        public List<Vector2> BorderPoints
        {
            get => borderPoints;            
            
        }
        
        public PlayerZoneService Service { get => service; }

        [System.Serializable]
        public class Settings
        {         
            public float initialRadius;
            public int initialDotsCount;
            public Material debugFirstPoint;
            public Material debugSecondPoint;
            public Material debugOtherPoints;
            
        }
        public int GetNearestBorderPointTo(Vector2 position)
        {

           return Helpers.GetNearestBorderPointTo(borderPoints, position);
        }
        public bool  WithinBorder(Vector2 pos)
        {
            return Helpers.CheckIfInPolygon(borderPoints, pos);
        }
        public void GenerateCirclePolygon()
        {
            SetBorder(
                    Helpers.GenerateCirclePolygon
                    (
                        settings.initialRadius,
                        settings.initialDotsCount,
                        Facade.Position2D
                     )
                );
        }

        public void Initialize()
        {
            GenerateCirclePolygon();
            view.Initialize();
            view.UpdateMesh();
        }

        internal void SetBorder(List<Vector2> border)
        {
            borderPoints = border;
            if (borderPoints.Count > 0)
            {
                zoneRect.InitWithPosition(border[0]);
                foreach (var item in borderPoints)
                {
                    zoneRect.UpdateWithPosition(item);
                }
            }

        }
        public void Tick()
        {
            
            Debug.DrawLine(new Vector3(zoneRect.left, zoneRect.bottom), new Vector3(zoneRect.right, zoneRect.top), Color.black);

        }
    }
}