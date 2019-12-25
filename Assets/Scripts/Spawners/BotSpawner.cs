using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class BotSpawner
    {
        readonly PlayerFacade.BotFactory _playerFactory;

        public BotSpawner(PlayerFacade.BotFactory playerFactory) 
        {
            _playerFactory = playerFactory;
        }



        public void SpawnBot(Vector3 pos)
        {
            var player = _playerFactory.Create(pos, 0);
        }

    }
}