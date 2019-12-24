using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{
    public class GameController : IInitializable, ITickable, IDisposable
    {
        [Inject]
        ControlablePlayerSpawner _playerSpawner;
        public void Dispose()
        {

        }

        public void Initialize()
        {
            _playerSpawner.SpawnPlayer();
        }

        public void Tick()
        {
        }
    }
}