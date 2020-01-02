using PaperIOClone.Player.Bot;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Player
{
    public class PlayerFacade : MonoBehaviour,
        IPoolable<Vector3, Color, string, IMemoryPool>,
        IPoolable<Vector3, Color, string, BotAiPreset, IMemoryPool>
    {
        private IMemoryPool _pool;
        private PlayersRegistry _registry;
        private PlayerRunner _runner;
        private SignalBus _signalBus;
        [SerializeField] private GameObject crownContainer;
        public BotAiPreset Preset { get; private set; }

        public string Name { get; private set; }

        public Color MainColor { get; private set; }

        public bool IsBot => Preset != null;

        public PlayerZone Zone { get; private set; }

        public Vector2 LastHomePosition => _runner.LastInsideHomePosition;

        public bool InsideHome => _runner.InsideHome;

        public Vector3 Position
        {
            get => _runner.Position;
            private set => _runner.Position = value;
        }

        public float Rotation => _runner.Rotation;

        public Vector3 LookDir => _runner.LookDir;

        public PlayerLine Line { get; private set; }

        public int Kills { get; private set; }

        public void OnSpawned(Vector3 position, Color color, string playerName, BotAiPreset preset, IMemoryPool pool)
        {
            Preset = preset;
            OnSpawn(position, color, playerName, pool);
        }

        public void OnSpawned(Vector3 position, Color color, string playerName, IMemoryPool pool)
        {
            OnSpawn(position, color, playerName, pool);
        }

        public void OnDespawned()
        {
            Kills = 0;
            Line.ClearLine();
            _registry.RemovePlayer(this);
        }


        [Inject]
        public void Construct(
            PlayerRunner player,
            PlayersRegistry playersRegistry,
            PlayerLine line,
            PlayerZone zone,
            SignalBus signalBus
        )
        {
            _signalBus = signalBus;
            _runner = player;
            _registry = playersRegistry;
            Zone = zone;
            Line = line;
        }

        public void SetCrown(bool isOn)
        {
            crownContainer.SetActive(isOn);
        }

        private void OnSpawn(Vector3 position, Color color, string playerName, IMemoryPool pool)
        {
            _pool = pool;
            MainColor = color;
            _registry.AddPlayer(this);
            Name = playerName;
            gameObject.name = Name;
            _runner.OnSpawn();
            Position = position;
            Zone.OnSpawn();
        }

        public void Die()
        {
            _signalBus.Fire(new SignalDie());
            _pool.Despawn(this);
        }

        public void OnKill()
        {
            Kills++;
        }

        public class PlayerFactory : PlaceholderFactory<Vector3, Color, string, PlayerFacade>
        {
        }

        public class BotFactory : PlaceholderFactory<Vector3, Color, string, BotAiPreset, PlayerFacade>
        {
        }
    }
}