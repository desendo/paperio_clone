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



        public void SpawnPlayer()
        {
            var player = _playerFactory.Create(0, 0);
        }

    }
}