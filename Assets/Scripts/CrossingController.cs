using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System;

namespace Game
{
    
    public class CrossingController
    {

        public class Crossing
        {
            public PlayerLine line;
            public PlayerZone zone;
            public int crossIndex;
            public bool isEntry;
            public bool crossed;
        }

        public List<Crossing> crossings; 

        [Inject]
        PlayersRegistry playersRegistry;

        public CrossingController(PlayersRegistry playersRegistry)
        {
            this.playersRegistry = playersRegistry;
            crossings = new List<Crossing>();
        }

        public void OnLinePointAdded(PlayerLine line)
        {
            //Debug.Log("OnLinePointAdded " + line.Facade.name);
            HandleLineCrossings(line);
            HandleZoneCrossings(line);            
        }

        public void ClearNullCrossings()
        {
            var crossingsArray = crossings.ToArray();
            for (int i = 0; i < crossingsArray.Length; i++)
            {
                if (crossingsArray[i].line == null||
                    crossingsArray[i].zone == null)
                {
                    crossings.Remove(crossingsArray[i]);
                }
            }
        }
        public void RemoveCrossings(PlayerFacade facade)
        {
            var crossingsArray = crossings.ToArray();
            for (int i = 0; i < crossingsArray.Length; i++)
            {
                if (crossingsArray[i].line == facade.Line ||
                    crossingsArray[i].zone == facade.Zone)
                {
                    crossings.Remove(crossingsArray[i]);
                }
            }
        }

        private void HandleZoneCrossings(PlayerLine line)
        {
            int i = 0;
            foreach (var zone in playersRegistry.Zones)
            {
                
                
                if (line.Facade.Zone == zone) continue;
                if (!zone.rect.Overlaps(line.rect, 0.5f)) continue;

                Vector2 crossing = Vector2.zero;

                if (Helpers.SegmentCrossesPolyline(
                    line.LastSegment,
                    zone.BorderPoints,
                    ref crossing))
                {
                    int crossIndex = line.InsertPointToLastSegment(crossing);

                    bool isEntry = zone.WithinBorder(line.LastPoint);                    
                    AddCrossing(line, zone, crossIndex, isEntry);
                    
                      //var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                      //go.transform.localScale *= 0.5f;
                      //go.transform.position = crossing;
                }               
            }
        }
        public void PerformCuts( PlayerLine line)
        {
            
            foreach (var zone in playersRegistry.Zones)
            {
                List<Crossing> zoneCrossings = new List<Crossing>();


                foreach (var item in crossings)
                {
                    if (item.line == line && item.zone == zone)
                    {
                        zoneCrossings.Add(item);
                    }
                }
                zone.Service.PerfomCuts(zoneCrossings);
            }

            
        }
        private void AddCrossing(PlayerLine line,PlayerZone zone, int crossIndex, bool isEntry)
        {
            crossings.Add(new Crossing()
            {
                crossIndex = crossIndex,
                isEntry = isEntry,
                line = line,
                zone = zone
            });
        }

        private void HandleLineCrossings(PlayerLine line)
        {
            int count = line.LineDots.Count;
            if (count < 2) return;
            var linesArray = playersRegistry.Lines.ToArray();
            foreach (var otherLine in linesArray)
            {
                Vector2 crossing = Vector2.zero;
                if (Helpers.SegmentCrossesPolyline(
                    line.LastSegment,
                    otherLine.LineDots,
                    ref crossing))
                {                    
                    HandleCutOff(otherLine);                    
                    HandleKill(line, otherLine);
                    Debug.Log(crossing);
                    var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.localScale *= 0.3f;
                    go.transform.position = crossing;
                }

                
            }
        }

        private static void HandleKill(PlayerLine lineKiller, PlayerLine killedLine)
        {
            killedLine.Facade.Die();
        }

        private static void HandleCutOff(PlayerLine otherLine)
        {
            otherLine.Facade.CutOff();
        }
    }
}
