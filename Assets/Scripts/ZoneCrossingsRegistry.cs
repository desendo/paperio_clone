using System.Collections.Generic;
using UnityEngine;
namespace Game
{

    public class ZoneCrossingData
    {
        public bool isEntry;
        public Vector3 position;
        public PlayerZone zone;
    }

    public class ZoneCrossingsRegistry
    {        
        readonly Dictionary<PlayerRunnerView, ZoneCrossingData> crossingDataDictionary = new Dictionary<PlayerRunnerView, ZoneCrossingData>();        
        
        public void AddCross(PlayerRunnerView runner)
        {

        }
        public void RemovePlayer(PlayerFacade player)
        {

        }
    }
}
