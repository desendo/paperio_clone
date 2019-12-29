using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;
namespace Game
{
    public class GameController : IInitializable
    {
        [Inject]
        ControlablePlayerSpawner _playerSpawner;
        [Inject]
        BotSpawner _botSpawner;
        [Inject]
        PlayersRegistry registry;
        [Inject]
        World world;
        [Inject]
        Settings settings;
        [Inject]
        PlayerZone.Settings playerZoneSettings;
        public void Initialize()
        {
            var bot_spawner = Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(x => 
            {
                if (CurrentBotsCount < settings.maximumBots)
                {
                    var v = UnityEngine.Random.insideUnitCircle * (world.Radius - playerZoneSettings.initialRadius);
                    _botSpawner.SpawnBot(v);

                }
            });

            var player_spawner = Observable.Interval(TimeSpan.FromMilliseconds(1000)).Subscribe(x =>
            {
                if (CurrentPlayerCount < 1)
                {
                    var v = UnityEngine.Random.insideUnitCircle * (world.Radius - playerZoneSettings.initialRadius);
                    _playerSpawner.SpawnPlayer(v);
                }
            });

        }
        public int CurrentBotsCount
        {
            get
            {
                int c = 0;
                for (int i = 0; i < registry.PlayerFacades.Count; i++)
                {
                    if (registry.PlayerFacades[i].IsBot)
                        c++;
                }
                return c;
            }
        }
        public int CurrentPlayerCount
        {
            get
            {
                int c = 0;
                for (int i = 0; i < registry.PlayerFacades.Count; i++)
                {
                    if (!registry.PlayerFacades[i].IsBot)
                        c++;
                }
                return c;
            }
        }


        [System.Serializable]
        public class Settings
        {
            public int maximumBots;
        }
    }
}