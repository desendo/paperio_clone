using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerFacade : MonoBehaviour, IPoolable<float, float, IMemoryPool>, IDisposable
    {
        Player _model;
        PlayersRegistry _registry;

        [Inject]
        public void Construct(Player player, PlayersRegistry playersRegistry)
        {
            _model = player;
            _registry = playersRegistry;
        }

        public bool IsDead
        {
            get { return _model.IsDead; }
        }
        public bool IsOutSide
        {
            get { return _model.IsOutside; }
        }
        public bool IsCutOf
        {
            get { return _model.IsCutOf; }
        }
        public Vector2 Position
        {
            get { return _model.Position; }
            set { _model.Position = value; }
        }
        public float Rotation
        {
            get { return _model.Rotation; }
        }
        public void OnSpawned(float accuracy, float speed, IMemoryPool pool)
        {
            Debug.Log("OnSpawned " + name);
            //_pool = pool;
            //_tunables.Accuracy = accuracy;
            //_tunables.Speed = speed;

            _registry.AddPlayer(this);
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