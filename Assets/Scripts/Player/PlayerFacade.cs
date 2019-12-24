﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerFacade : MonoBehaviour, IPoolable<float, float, IMemoryPool>, IDisposable
    {

        PlayerRunner _runner;
        PlayerZone _zone;
        
        PlayerLine _line;
        PlayersRegistry _registry;

        [Inject]
        public void Construct(PlayerRunner player, PlayersRegistry playersRegistry, PlayerLine line, PlayerZone zone)
        {
            _runner = player;            
            _registry = playersRegistry;
            _zone = zone;
            _line = line;
        }
        public void OnSpawned(float accuracy, float speed, IMemoryPool pool)
        {

            _registry.AddPlayer(this);
        }

        public void OnDespawned()
        {
            _registry.RemovePlayer(this);
        }

        public void Dispose()
        {
        }
        public PlayerZone Zone
        {
            get => _zone;
        }
        public PlayerLine Line
        {
            get => _line;
        }
        public class PlayerFactory : PlaceholderFactory<float, float, PlayerFacade>
        {
        }
        public class BotFactory : PlaceholderFactory<float, float, PlayerFacade>
        {
        }
    }
}