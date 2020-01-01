using System;
using PaperIOClone.Helpers;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Player
{
    public class PlayerMoveHandler : ITickable, ILateTickable
    {
        private readonly InputState _inputState;
        private readonly PlayerRunner _player;
        private readonly Settings _settings;
        private readonly World _world;
        private float _angle;
        private Transform _cameraTransform; //todo камеру вытащить
        private Vector2 _currentRotatePoint;

        public PlayerMoveHandler(Settings settings, PlayerRunner player, InputState inputState, World world)
        {
            _settings = settings;
            _player = player;
            _inputState = inputState;
            _world = world;
        }


        public void LateTick()
        {
            if (_cameraTransform == null) _cameraTransform = Camera.main.transform;

            _cameraTransform.position =
                new Vector3(_player.Position.x, _player.Position.y, Camera.main.transform.position.z);
        }

        public void Tick()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            var frameRateFactor = 60f * Time.deltaTime;
            _player.Position += _player.LookDir * (_settings.moveSpeed * frameRateFactor);

            _player.Position = Geometry.TrimPositionToWorldBounds(_player.Position, _world.Radius, _world.Center);

            if (_inputState.TotalDelta.sqrMagnitude > _settings.swipeDeadZoneLenght * _settings.swipeDeadZoneLenght)
            {
                var angle = Mathf.Atan2(_inputState.TotalDelta.normalized.y, _inputState.TotalDelta.normalized.x) *
                            Mathf.Rad2Deg;
                SetTargetAngle(angle);
            }

            if (!(Mathf.Abs(_player.Rotation - _angle) > _settings.rotationEpslon)) return;

            var lerpFactor = _settings.turnSpeed * frameRateFactor;
            _player.Rotation = Mathf.LerpAngle(_player.Rotation, _angle, lerpFactor);
        }

        private void SetTargetAngle(float angle)
        {
            _angle = angle;
        }

        [Serializable]
        public class Settings
        {
            public float moveSpeed;
            public float rotationEpslon;
            public float swipeDeadZoneLenght;
            public float turnSpeed;
        }
    }
}