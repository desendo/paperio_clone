using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class ControlablePlayerSpawner :  ITickable, IInitializable
    {
        readonly PlayerFacade.PlayerFactory _playerFactory;

        public ControlablePlayerSpawner(PlayerFacade.PlayerFactory playerFactory) 
        {
            _playerFactory = playerFactory;
        }

        public void Initialize()
        {
            var player = _playerFactory.Create(0, 0);
        }

        public void Tick()
        {
        }
    }
}