using System.Collections.Generic;
using System.Linq;
using PaperIOClone.Player;
using UnityEngine;
using Zenject;

namespace PaperIOClone
{
    public class ScoresHandler
    {
        private readonly GuiHandler _guiHandler;
        private readonly PlayersRegistry _playersRegistry;
        private readonly SignalBus _signalBus;
        private readonly World _world;

        public ScoresHandler(SignalBus signalBus, World world, PlayersRegistry playersRegistry, GuiHandler guiHandler)
        {
            _signalBus = signalBus;
            _world = world;
            _playersRegistry = playersRegistry;
            _guiHandler = guiHandler;

            signalBus.Subscribe<SignalDie>(UpdateScores);
            signalBus.Subscribe<SignalZoneChanged>(UpdateScores);
        }

        private void UpdateScores()
        {
            var players = new List<PlayerFacade>(_playersRegistry.PlayerFacades);

            players = players.OrderByDescending(x => x.Zone.Area).ToList();

            var scoreData = new List<ScoreData>();

            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];

                player.SetCrown(i == 0);

                scoreData.Add(new ScoreData
                    {
                        AreaNormalized = player.Zone.Area / _world.Area,
                        Color = player.MainColor,
                        IsPlayer = !player.IsBot,
                        Kills = player.Kills,
                        Name = player.name
                    }
                );
            }

            _guiHandler.SetScores(scoreData);
        }

        public struct ScoreData
        {
            public float AreaNormalized;
            public int Kills;
            public Color Color;
            public string Name;
            public bool IsPlayer;
        }
    }
}