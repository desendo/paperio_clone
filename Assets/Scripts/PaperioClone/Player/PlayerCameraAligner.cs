using UnityEngine;
using Zenject;

namespace PaperIOClone.Player
{
    public class PlayerCameraAligner : ILateTickable
    {
        private readonly PlayerRunner _player;
        private Transform _cameraTransform;

        public PlayerCameraAligner(PlayerRunner player)
        {
            _player = player;
        }

        public void LateTick()
        {
            if (_cameraTransform == null)
            {
                if (Camera.main == null)
                    return;
                _cameraTransform = Camera.main.transform;
            }

            _cameraTransform.position =
                new Vector3(_player.Position.x, _player.Position.y, _cameraTransform.position.z);
        }
    }
}