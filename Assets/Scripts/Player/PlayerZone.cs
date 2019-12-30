using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerZone
    {
        [Inject]
        Settings settings;
        [Inject]
        public PlayerFacade Facade { get; }
        public PlayerZoneView view { get; private set; }
        private List<Vector2> borderPoints;
        private float _area;

        Rect zoneRect;
        [Inject]
        public void Construct(PlayerZoneView view)
        {

            this.view = view;
            borderPoints = new List<Vector2>();
            zoneRect = new Rect();
        }
        public float GetArea()
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
        public float Area { get => _area; private set => _area = value; }

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
                    (settings.initialRadius,
                    settings.initialDotsCount,
                    Facade.Position)
                );
        }

        public void OnSpawn()
        {
            GenerateCirclePolygon();
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

            Area = GetArea();

            SignalsController.Default.Send(new SignalZoneChanged() );

        }

    }
}