using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerMoveHandler : ITickable,ILateTickable
    {
        readonly Settings _settings;
        readonly PlayerRunner _player;
        readonly InputState _inputState;

        private Vector2 currentRotatePoint;
        public PlayerMoveHandler(Settings settings, PlayerRunner player, InputState inputState)
        {
            _settings = settings;
            _player = player;
            _inputState = inputState;            
        }
        float angle;
        public void Tick()
        {
            HandleMovement();
        }
        void HandleMovement()
        {
            float frameRateFactor = 60f * Time.deltaTime;
            _player.Position += _player.LookDir * _settings.moveSpeed * frameRateFactor;

            if (_inputState.totalDelta.sqrMagnitude > _settings.swipeDeadZoneLenght * _settings.swipeDeadZoneLenght)
                angle = Mathf.Atan2(_inputState.totalDelta.normalized.y, _inputState.totalDelta.normalized.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(_player.Rotation - angle) > _settings.rotationEpslon)
            {
                float lerpFactor = _settings.turnSpeed * frameRateFactor;
                _player.Rotation = Mathf.LerpAngle(_player.Rotation, angle, lerpFactor);
                
                //_inputState.totalDelta = Vector2.Lerp(_inputState.totalDelta, Vector2.zero, lerpFactor);
            }
        }
        Transform cameraTransform;
        public void LateTick()
        {
            if (cameraTransform == null)
                cameraTransform = Camera.main.transform;

            cameraTransform.position = new Vector3(_player.Position.x, _player.Position.y, Camera.main.transform.position.z);
        }

        [Serializable]
        public class Settings
        {
            public float moveSpeed;
            public float turnSpeed;
            public float rotationEpslon;
            public float swipeDeadZoneLenght;
        }

    }
}