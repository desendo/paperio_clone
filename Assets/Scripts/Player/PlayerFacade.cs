using System;
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

        public bool IsDead
        {
            get { return _runner.IsDead; }
        }
        public bool IsOutSide
        {
            get { return _runner.IsOutside; }
        }
        public bool IsCutOf
        {
            get { return _runner.IsCutOf; }
        }
        public Vector2 Position
        {
            get { return _runner.Position; }
            set { _runner.Position = value; }
        }
        public float Rotation
        {
            get { return _runner.Rotation; }
        }
        public void OnSpawned(float accuracy, float speed, IMemoryPool pool)
        {
            Debug.Log("OnSpawned " + name);
            //_pool = pool;
            //_tunables.Accuracy = accuracy;
            //_tunables.Speed = speed;

            _registry.AddPlayer(this);
        }
        public IEnumerable<Vector2> Line
        {
            get { return _line.LineDots; }
        }
        public void OnDespawned()
        {
            _registry.RemovePlayer(this);
        }

        public void Dispose()
        {
        }

        public class PlayerFactory : PlaceholderFactory<float, float, PlayerFacade>
        {
        }
        public class BotFactory : PlaceholderFactory<float, float, PlayerFacade>
        {
        }
    }
}