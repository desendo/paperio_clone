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
            public PlayerZone zone;
            public int enterIndex;
            public int exitIndex;
            public bool passed;
            public bool performed;
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

        


        private void HandleZoneCrossings(PlayerLine line)
        {
            int i = 0;
            foreach (var zone in playersRegistry.Zones)
            {
                
                
                if (line.Facade.Zone == zone) continue;
                if (!zone.rect.Overlaps(line.rect, 0.5f)) continue;

                Vector2 crossing = Vector2.zero;
                bool isCrossing = Helpers.SegmentCrossesPolyline(line.LastSegment, zone.BorderPoints, ref crossing);
                if (isCrossing)
                {
                    int crossIndex = line.InsertPointToLastSegment(crossing);

                    bool isEntry = zone.WithinBorder(line.LastPoint);                    
                    AddCrossing(line, zone, crossIndex, isEntry);
                    
                      var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                      go.transform.localScale *= 0.2f;
                      go.transform.position = crossing;
                    if(isEntry)
                        go.GetComponent<MeshRenderer>().material.color = Color.red;
                    else
                        go.GetComponent<MeshRenderer>().material.color = Color.blue;
                }               
            }
        }
        public void PerformCuts( PlayerLine line)
        {
            
            foreach (var item in line.Crossings)
            {
                if (item.performed) continue;
                if (!item.passed) continue;
                item.zone.Service.PerfomCut(line, item.enterIndex, item.exitIndex);
                item.performed = true;


            }
            
            
        }
        private void AddCrossing(PlayerLine line,PlayerZone zone, int crossIndex, bool isEntry)
        {
            line.AddCrossingWithZone(zone, crossIndex, isEntry);
            
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
