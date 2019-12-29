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
        public ScoresHandler()
        {
            SignalsController.Default.Add(this);
            Debug.Log("ScoresHandler");
        }
        ~ScoresHandler()
        {
            SignalsController.Default.Remove(this);
        }
        public void HandleSignal(SignalZoneChanged arg)
        {
            //List<PlayerZone> SortedList = playersRegistry.Zones.OrderByDescending(o => o.Area).ToList();
            //Debug.Log("zone changed " + arg.area / world.Area * 100f +"%");
            UpdateScores();


        }

        public void HandleSignal(SignalDie arg)
        {
            
        }

        

        private void UpdateScores()
        {
            List<PlayerFacade> players = new List<PlayerFacade>(playersRegistry.PlayerFacades);

            players = players.OrderBy(x => x.Zone.Area).ToList();

        }
        public struct ScoreData
        {
            public float areaNormalized;
            public int kills;

        }
    }
}