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
        readonly World _world;

        private Vector2 currentRotatePoint;

        public BotMoveHandler(PlayerMoveHandler.Settings settings, PlayerRunner player, TargetAngleState angle, World world)
        {
            _settings = settings;
            _player = player;
            _angle = angle;
            _world = world;
        }
        
        public void Tick()
        {
            HandleMovement();
        }
        void HandleMovement()
        {
            float frameRateFactor = 60f * Time.deltaTime;
            _player.Position += _player.LookDir * _settings.moveSpeed * frameRateFactor;
            _player.Position = Helpers.TrimPositionToWorldBounds(_player.Position, _world.Radius, _world.Center);

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