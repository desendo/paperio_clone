using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
namespace Game
{
    public class ScoresHandler : IReceive<SignalDie>, IReceive<SignalZoneChanged>
    {
        [Inject]
        PlayersRegistry playersRegistry;
        [Inject]
        World world;
        [Inject]
        GuiHandler guiHandler;
        public ScoresHandler()
        {
            SignalsController.Default.Add(this);
        }
        ~ScoresHandler()
        {
            SignalsController.Default.Remove(this);
        }
        public void HandleSignal(SignalZoneChanged arg)
        {
            UpdateScores();
        }

        public void HandleSignal(SignalDie arg)
        {
            UpdateScores();
        }

        

        private void UpdateScores()
        {
            List<PlayerFacade> players = new List<PlayerFacade>(playersRegistry.PlayerFacades);

            players = players.OrderByDescending(x => x.Zone.Area).ToList();

            List<ScoreData> scoreData = new List<ScoreData>();

            for (int i = 0; i < players.Count; i++)
            {
                

                PlayerFacade player = players[i];

                if (i == 0)
                    player.SetCrown(true);
                else
                    player.SetCrown(false);

                scoreData.Add(new ScoreData()
                {
                    areaNormalized = player.Zone.Area / world.Area,
                    color = player.MainColor,
                    isPlayer = !player.IsBot,
                    kills = player.Kills,
                    name = player.name
                }
                );
            }
            guiHandler.SetScores(scoreData);


        }
        public struct ScoreData
        {
            public float areaNormalized;
            public int kills;
            public Color color;
            public string name;
            public bool isPlayer;

        }
    }
}