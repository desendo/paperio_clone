﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerFacade : MonoBehaviour, IPoolable<Vector3, float, IMemoryPool>, IDisposable
    {

        PlayerRunner _runner;
        PlayerZone _zone;
        PlayerLine _line;
        PlayersRegistry _registry;

        IMemoryPool _pool;

        [Inject]
        public void Construct(
            PlayerRunner player,
            PlayersRegistry playersRegistry,
            PlayerLine line,
            PlayerZone zone)
        {
            _runner = player;
            _registry = playersRegistry;
            _zone = zone;
            _line = line;
        }
        public void OnSpawned(Vector3 position, float speed, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            _registry.AddPlayer(this);
        }

        public void OnDespawned()
        {
            _registry.RemovePlayer(this);
        }

        public void Dispose()
        {

        }
        public void Die()
        {
            Debug.Log("Die");
            _pool.Despawn(this);
        }

        public PlayerZone Zone
        {
            get => _zone;
        }
        public Vector2 Position2D
        {
            get => transform.position;
        }
        public Vector3 Position
        {
            get => transform.position;
        }
        public void CutOff()
        {
           // _runner.CutOff();
        }

        public PlayerLine Line
        {
            get => _line;
        }
        public class PlayerFactory : PlaceholderFactory<Vector3, float, PlayerFacade>
        {
        }
        public class BotFactory : PlaceholderFactory<Vector3, float, PlayerFacade>
        {
        }
    }
}