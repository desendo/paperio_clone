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
        readonly Dictionary<PlayerRunner, ZoneCrossingData> crossingDataDictionary = new Dictionary<PlayerRunner, ZoneCrossingData>();        
        
        public void AddCross(PlayerRunner runner)
        {

        }
        public void RemovePlayer(PlayerFacade player)
        {

        }
    }
}
