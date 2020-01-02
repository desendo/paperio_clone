using System;
using System.Collections.Generic;
using PaperIOClone.Helpers;
using PaperIOClone.Installers;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Player
{
    public class PlayerRunner : ITickable, IInitializable
    {
        private CrossingController _crossingController;
        private GameSettingsInstaller.DebugSettings _debugSettings;
        private int _enterPointIndex;
        private Vector3 _entryPoint;

        private Vector3 _exitPoint;
        private int _exitPointIndex;
        private PlayerFacade _facade;

        private PlayerLine _line;
        private Vector3 _lookDir;
        private bool _oldInside;

        private Vector3 _oldPosition;
        private PlayersRegistry _playersRegistry;
        private Vector3 _position;
        private float _rotation;
        private Settings _settings;
        private float _squaredDeltaPos;
        private PlayerRunnerView _view;

        private bool _wasOutsideHomeZone;
        private PlayerZone _zone;
        private PlayerZoneService _zoneService;
        private PlayerZoneView _zoneView;

        public Vector2 LastInsideHomePosition { get; private set; }

        public Vector3 LookDir => new Vector3(Mathf.Cos(Rotation * Mathf.Deg2Rad), Mathf.Sin(Rotation * Mathf.Deg2Rad));

        public float Rotation
        {
            get => _rotation;
            set
            {
                _view.Rotation = value;
                _rotation = value;
            }
        }

        public bool InsideHome => Geometry.CheckIfInPolygon(_zone.BorderPoints, Position);

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                _view.Position = value;
            }
        }

        public void Initialize()
        {
            _oldPosition = Position;
            _oldInside = true;
            LastInsideHomePosition = Position;
        }

        public void Tick()
        {
            CheckCrossings();
        }

        [Inject]
        public void Constructor(PlayerLine line, PlayerRunnerView view, PlayerZone zone, PlayerZoneView zoneView,
            PlayerZoneService zoneService, CrossingController crossingController, PlayerFacade facade,
            GameSettingsInstaller.DebugSettings debugSettings, Settings settings)
        {
            _line = line;
            _view = view;
            _zone = zone;
            _zoneView = zoneView;
            _zoneService = zoneService;
            _crossingController = crossingController;
            _facade = facade;
            _debugSettings = debugSettings;
            _settings = settings;
        }

        public void OnSpawn()
        {
            _oldPosition = Position;
            _oldInside = true;
            LastInsideHomePosition = Position;
            Rotation = 0;
        }

        private void CheckCrossings()
        {
            var deltaPosition = (Position - _oldPosition).magnitude;

            if (!(deltaPosition > _settings.unitPerPoint)) return;
            CheckHomeZoneCrossing();
            CheckLinesCrossing();
            _oldPosition = Position;
        }

        private void CheckLinesCrossing()
        {
            _crossingController.CheckLineCrossings(_oldPosition, Position, _facade);
        }

        private void CheckHomeZoneCrossing()
        {
            var isInside = InsideHome; //кешируем 
            var borderCrossingEdgeIndexes = new List<int>();

            var isCrossing = Geometry.SegmentCrossesPolyline(Position, _oldPosition, _zone.BorderPoints,
                out var crossPoint, borderCrossingEdgeIndexes);

            if (_oldInside != isInside && !isCrossing)
            {
                Debug.LogWarning(_facade.Name + " переход без фиксации пересечения. возможно ошибка построения меша");
                Debug.LogWarning(_facade.Name + " внутри: " + InsideHome);

                _facade.Die(); //ошибка у бота. убиваем.
            }

            if (_oldInside != isInside && isCrossing)
            {
                if (!isInside) LastInsideHomePosition = _oldPosition;

                var modifiedBorder = new List<Vector2>();
                _oldInside = isInside;
                for (var i = 0; i < _zone.BorderPoints.Count; i++)
                {
                    modifiedBorder.Add(_zone.BorderPoints[i]);
                    if (borderCrossingEdgeIndexes.Count > 1 && i == borderCrossingEdgeIndexes[0])
                        modifiedBorder.Add(crossPoint);
                }

                _zone.SetBorder(modifiedBorder);

                _line.AddDot(crossPoint);
                if (isInside) HandleHomeZoneEnter();
            }
            else if (!isInside)
            {
                _line.AddDot(Position);
            }
        }

        private void HandleHomeZoneEnter()
        {
            _zoneService.HandleEnterHomeZone();
            _zoneView.UpdateMesh();
            _line.ClearLine();
            _crossingController.PerformCuts(_zone);
        }

        [Serializable]
        public class Settings
        {
            public float unitPerPoint;
        }
    }
}