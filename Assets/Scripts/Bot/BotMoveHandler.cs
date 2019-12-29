using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class BotMoveHandler : ITickable
    {
        readonly PlayerMoveHandler.Settings _settings;
        readonly PlayerRunner _player;
        readonly TargetAngleState _angle;

        private Vector2 currentRotatePoint;

        public BotMoveHandler(PlayerMoveHandler.Settings settings, PlayerRunner player, TargetAngleState angle)
        {
            _settings = settings;
            _player = player;
            _angle = angle;
        }
        
        public void Tick()
        {
            HandleMovement();
        }
        void HandleMovement()
        {
            float frameRateFactor = 60f * Time.deltaTime;
            _player.Position += _player.LookDir * _settings.moveSpeed * frameRateFactor;


            if (Mathf.Abs(_player.Rotation - _angle.angle) > _settings.rotationEpslon)
            {
                float lerpFactor = _settings.turnSpeed * frameRateFactor;
                _player.Rotation = Mathf.LerpAngle(_player.Rotation, _angle.angle, lerpFactor);
                
            }
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