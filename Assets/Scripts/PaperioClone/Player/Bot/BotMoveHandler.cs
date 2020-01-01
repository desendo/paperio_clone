using System;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Player.Bot
{
    public class BotMoveHandler : ITickable
    {
        private readonly TargetAngleState _angle;
        private readonly PlayerRunner _player;
        private readonly PlayerMoveHandler.Settings _settings;
        private readonly World _world;

        public BotMoveHandler(PlayerMoveHandler.Settings settings, PlayerRunner player, TargetAngleState angle,
            World world)
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

        private void HandleMovement()
        {
            var frameRateFactor = 60f * Time.deltaTime;
            _player.Position += _player.LookDir * (_settings.moveSpeed * frameRateFactor);
            _player.Position =
                Helpers.Geometry.TrimPositionToWorldBounds(_player.Position, _world.Radius, _world.Center);

            if (Mathf.Abs(_player.Rotation - _angle.Angle) > _settings.rotationEpslon)
            {
                var lerpFactor = _settings.turnSpeed * frameRateFactor;
                _player.Rotation = Mathf.LerpAngle(_player.Rotation, _angle.Angle, lerpFactor);
            }
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