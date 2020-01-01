using PaperIOClone.Player;
using UnityEngine;

namespace PaperIOClone.Spawners
{
    public class ControlablePlayerSpawner
    {
        private readonly PlayerFacade.PlayerFactory _playerFactory;

        public ControlablePlayerSpawner(PlayerFacade.PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public void SpawnPlayer(Vector3 pos)
        {
            var player = _playerFactory.Create(pos, Helpers.Geometry.GetRandomColor(), "Player");
        }
    }
}