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
        [Inject]
        BotSpawner _botSpawner;
        public void Dispose()
        {

        }

        public void Initialize()
        {
            _playerSpawner.SpawnPlayer( new Vector3(10,10,0));
         //   _botSpawner.SpawnBot(new Vector3(20, 13, 0));
        }

        public void Tick()
        {
        }
    }
}