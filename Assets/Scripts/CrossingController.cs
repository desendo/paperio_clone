using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System;

namespace Game
{
    
    public class CrossingController
    {

        [Inject]
        PlayersRegistry playersRegistry;

        public CrossingController(PlayersRegistry playersRegistry)
        {
            this.playersRegistry = playersRegistry;
        }


        public void OnLinePointAdded(PlayerLine line)
        {            
            HandleLineCrossings(line);            
        }

        public void PerformCuts( PlayerZone zone)
        {
            foreach (var zoneToCut in playersRegistry.Zones)
            {
                if (zoneToCut == zone) continue; // свою зону не отрезаем
                bool overlaps = zone.rect.Overlaps(zoneToCut.rect);
                if(overlaps)
                    ZoneCross(zone, zoneToCut);
            } 
        }
        private void ZoneCross(PlayerZone cuttingZone, PlayerZone zoneToCut)
        {

            var cuttingLine = cuttingZone.BorderPoints.ToArray();
            var lineToCut = zoneToCut.BorderPoints.ToArray();


            List<int> crossingIndexes = new List<int>(); 
            List<Vector2> crossingPoints = new List<Vector2>();

            bool hasCross = false;

            for (int i = 0; i < cuttingLine.Length; i++)
            {
                if (Helpers.CheckIfInPolygon(zoneToCut.BorderPoints, cuttingLine[i]))
                {
                    hasCross = true;
                    break;
                }
            }

            if (!hasCross) return;

            bool wasInside = false;
            bool isInside = false;

            List<Vector2> updatetCuttingLine = new List<Vector2>();
            for (int i = 0; i < cuttingLine.Length; i++)
            {

                int next = i + 1;
                if (i + 1 >= cuttingLine.Length)
                    next = 0;
                updatetCuttingLine.Add(cuttingLine[i]);
                Vector2 crossing = Vector2.zero;
                List<int> polylineIndex = new List<int>();
                if (Helpers.SegmentCrossesPolyline(cuttingLine[i], cuttingLine[next], zoneToCut.BorderPoints, ref crossing, polylineIndex))
                {
                    updatetCuttingLine.Add(crossing);
                }
            }

            int currentSegment = 0;
            List<List<Vector2>> segments = new List<List<Vector2>>();

            for (int i = 0; i < updatetCuttingLine.Count; i++)
            {

                if (Helpers.CheckIfInPolygon(zoneToCut.BorderPoints, updatetCuttingLine[i]))
                {
                    Debug.Log(currentSegment);
                    if (segments.Count < currentSegment + 1)
                    {

                        segments.Add(new List<Vector2>());
                    }
                    isInside = true;
                    segments[currentSegment].Add(updatetCuttingLine[i]);
                }
                else
                {
                    isInside = false;
                    if (isInside != wasInside)
                        currentSegment++;
                }
                wasInside = isInside;
            }
            for (int j = 0; j < segments.Count; j++)
            {
                zoneToCut.Service.PerfomCut(segments[j]);
            }

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
