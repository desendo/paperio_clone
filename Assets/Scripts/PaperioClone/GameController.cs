using System;
using PaperIOClone.Player;
using PaperIOClone.Spawners;
using UniRx;
using Zenject;
using Random = UnityEngine.Random;

namespace PaperIOClone
{
    public class GameController : IInitializable
    {
        private readonly BotSpawner _botSpawner;
        private readonly ControlablePlayerSpawner _playerSpawner;
        private readonly PlayerZone.Settings _playerZoneSettings;
        private readonly PlayersRegistry _registry;
        private readonly Settings _settings;
        private readonly World _world;

        public GameController(BotSpawner botSpawner, ControlablePlayerSpawner playerSpawner,
            PlayerZone.Settings playerZoneSettings, PlayersRegistry registry, Settings settings, World world)
        {
            _botSpawner = botSpawner;
            _playerSpawner = playerSpawner;
            _playerZoneSettings = playerZoneSettings;
            _registry = registry;
            _settings = settings;
            _world = world;
        }

        private int CurrentBotsCount
        {
            get
            {
                var botsCount = 0;
                for (var i = 0; i < _registry.PlayerFacades.Count; i++)
                    if (_registry.PlayerFacades[i].IsBot)
                        botsCount++;
                return botsCount;
            }
        }

        private int CurrentPlayerCount
        {
            get
            {
                var playerCount = 0;
                for (var i = 0; i < _registry.PlayerFacades.Count; i++)
                    if (!_registry.PlayerFacades[i].IsBot)
                        playerCount++;
                return playerCount;
            }
        }

        public void Initialize()
        {
            var botSpawner = Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(x =>
            {
                if (CurrentBotsCount > _settings.maximumBots - 1) return;
                var randomPosition = Random.insideUnitCircle * (_world.Radius - _playerZoneSettings.initialRadius);
                _botSpawner.SpawnBot(randomPosition);
            });

            var playerSpawner = Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(x =>
            {
                if (CurrentPlayerCount > 0) return;
                var randomPosition = Random.insideUnitCircle * (_world.Radius - _playerZoneSettings.initialRadius);
                _playerSpawner.SpawnPlayer(randomPosition);
            });
        }

        [Serializable]
        public class Settings
        {
            public int maximumBots;
        }
    }
}