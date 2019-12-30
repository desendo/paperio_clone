using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerFacade : MonoBehaviour,
        IPoolable<Vector3, Color, string, IMemoryPool>,        
        IPoolable<Vector3, Color, string, BotAIPreset, IMemoryPool>,
        IDisposable
    {

        PlayerRunner _runner;
        PlayerZone _zone;
        PlayerLine _line;
        PlayersRegistry _registry;

        public BotAIPreset preset { get; private set; }
        Color _color;
        IMemoryPool _pool;
        string _name;
        [Inject]
        CrossingController crossingController;
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
        public void OnSpawned(Vector3 position, Color color, string name, IMemoryPool pool)
        {
            OnSpawn(position, color, name, pool);

        }
        public void OnSpawned(Vector3 position, Color color, string name, BotAIPreset preset, IMemoryPool pool)
        {

            this.preset = preset;
            OnSpawn(position, color, name, pool);
        }

        public void SetCrown(bool isOn)
        {
            _runner.SetCrown(isOn);
        }

        private void OnSpawn(Vector3 position, Color color, string name, IMemoryPool pool)
        {
            _pool = pool;
            _color = color;
            _registry.AddPlayer(this);
            _name = name;
            gameObject.name = _name;

            //Line.ClearLine();
            _runner.OnSpawn();
            Position = position;
            Zone.OnSpawn();
        }


        public string Name
        {
            get=> _name;
        }
        public void OnDespawned()
        {
            Kills = 0;
            Line.ClearLine();
            _registry.RemovePlayer(this);            
        }
        public Color MainColor
        {
            get => _color;
        }
        public void Dispose()
        {
        }
        public bool IsBot
        {
            get => preset != null;
        }
        public float ZoneArea
        {
            get => _zone.GetArea();
        }
        public void Die()
        {            
            _pool.Despawn(this);
        }

        public PlayerZone Zone
        {
            get => _zone;
        }

        public Vector2 LastHomePosition
        {
            get => _runner.LastInsideHome;
        }
        public bool Inside
        {
            get => _runner.Inside;
        }
        public Vector3 Position
        {
            get => _runner.Position;
            set => _runner.Position = value;
        }
        public float Rotation
        {
            get => _runner.Rotation;            
        }
        public Vector3 LookDir
        {
            get => _runner.LookDir;
        }
        public void CutOff()
        {
           // _runner.CutOff();
        }
        public void OnKill()
        {
            Kills++;
        }
        public PlayerLine Line
        {
            get => _line;
        }
        public int Kills { get; private set; }

        public class PlayerFactory : PlaceholderFactory<Vector3, Color, string, PlayerFacade>
        {
        }
        public class BotFactory : PlaceholderFactory<Vector3, Color, string,BotAIPreset, PlayerFacade>
        {
        }        
    }
}