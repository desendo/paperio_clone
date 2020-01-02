using System;
using PaperIOClone.Helpers;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Player
{
    public class PlayerMoveHandler : ITickable
    {
        private readonly ITargetAngleState _angleState;
        private readonly PlayerRunner _player;
        private readonly Settings _settings;
        private readonly World _world;
        private float _angle;

        public PlayerMoveHandler(Settings settings, PlayerRunner player,
            ITargetAngleState angleState, World world)
        {
            _settings = settings;
            _player = player;
            _angleState = angleState;
            _world = world;
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


            _angle = _angleState.Angle;
            if (!(Mathf.Abs(_player.Rotation - _angle) > _settings.rotationEpslon)) return;

            var lerpFactor = _settings.turnSpeed * frameRateFactor;
            _player.Rotation = Mathf.LerpAngle(_player.Rotation, _angle, lerpFactor);
        }

        [Serializable]
        public class Settings
        {
            public float moveSpeed;
            public float rotationEpslon;
            public float turnSpeed;
        }
    }
}