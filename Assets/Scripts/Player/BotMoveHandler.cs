using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class BotMoveHandler : ITickable,ILateTickable
    {
        readonly PlayerMoveHandler.Settings _settings;
        readonly PlayerRunner _player;

        private Vector2 currentRotatePoint;
        private float angle;

        public BotMoveHandler(PlayerMoveHandler.Settings settings, PlayerRunner player)
        {
            _settings = settings;
            _player = player;
        }
        
        public void Tick()
        {
            HandleMovement();
        }
        void HandleMovement()
        {
            float frameRateFactor = 60f * Time.deltaTime;
            _player.Position += _player.LookDir * _settings.moveSpeed * frameRateFactor;

          //  if (_inputState.totalDelta.sqrMagnitude > _settings.swipeDeadZoneLenght * _settings.swipeDeadZoneLenght)
          //      angle = Mathf.Atan2(_inputState.totalDelta.normalized.y, _inputState.totalDelta.normalized.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(_player.Rotation - angle) > _settings.rotationEpslon)
            {
                float lerpFactor = _settings.turnSpeed * frameRateFactor;
                _player.Rotation = Mathf.LerpAngle(_player.Rotation, angle, lerpFactor);
                
                //_inputState.totalDelta = Vector2.Lerp(_inputState.totalDelta, Vector2.zero, lerpFactor);
            }
        }
        public void LateTick()
        {
        
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