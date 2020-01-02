using System;
using System.Collections.Generic;
using PaperIOClone.Helpers;
using UnityEngine;
using Zenject;
using Rect = PaperIOClone.Helpers.Rect;

namespace PaperIOClone.Player
{
    public class PlayerZone
    {
        private readonly Settings _settings;
        private readonly SignalBus _signalBus;

        public PlayerZone(PlayerZoneView view, SignalBus signalBus, PlayerFacade playerFacade, Settings settings)
        {
            View = view;
            _signalBus = signalBus;
            Facade = playerFacade;
            _settings = settings;

            BorderPoints = new List<Vector2>();
            Rect = new Rect();
        }

        public Rect Rect { get; }

        public List<Vector2> BorderPoints { get; private set; }

        public float Area { get; private set; }
        public PlayerFacade Facade { get; }
        public PlayerZoneView View { get; }

        public float GetArea()
        {
            return Mathf.Abs(Triangulator.Area(BorderPoints));
        }

        public int GetNearestBorderPointTo(Vector2 position)
        {
            return Geometry.GetNearestBorderPointTo(BorderPoints, position);
        }

        public bool WithinBorder(Vector2 pos)
        {
            return Geometry.CheckIfInPolygon(BorderPoints, pos);
        }

        private void GenerateCirclePolygon()
        {
            SetBorder(
                Geometry.GenerateCirclePolygon
                (_settings.initialRadius,
                    _settings.initialDotsCount,
                    Facade.Position)
            );
        }

        public void OnSpawn()
        {
            GenerateCirclePolygon();
            View.UpdateMesh();
        }

        internal void SetBorder(List<Vector2> border)
        {
            BorderPoints = border;
            if (BorderPoints.Count > 0)
            {
                Rect.InitWithPosition(border[0]);
                foreach (var item in BorderPoints) Rect.UpdateWithPosition(item);
            }

            Area = GetArea();
            _signalBus.Fire<SignalZoneChanged>();
        }

        [Serializable]
        public class Settings
        {
            public int initialDotsCount;
            public float initialRadius;
        }
    }
}