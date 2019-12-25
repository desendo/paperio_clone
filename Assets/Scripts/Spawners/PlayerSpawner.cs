using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class ControlablePlayerSpawner 

    {
        
        readonly PlayerFacade.PlayerFactory _playerFactory;

        public ControlablePlayerSpawner(PlayerFacade.PlayerFactory playerFactory) 
        {
            _playerFactory = playerFactory;
        }

        public void SpawnPlayer(Vector3 pos)
        {
            var player = _playerFactory.Create(pos, Helpers.GetRandomColor());
        }

    }
}