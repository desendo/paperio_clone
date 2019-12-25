using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System;

namespace Game
{
    
    public class LineCrossingController
    {
        [Inject]
        PlayersRegistry playersRegistry;
        public void DotAdded(PlayerLine line)
        {
            HandleCrossings(line);
        }

        private void HandleCrossings(PlayerLine line)
        {
            int count = line.LineDots.Count;
            if (count < 2) return;
            var linesArray = playersRegistry.Lines.ToArray();
            foreach (var otherLine in linesArray)
            {
                if (Helpers.SegmentCrossesPolyline(line.LineDots[count - 1], line.LineDots[count - 2], otherLine.LineDots))
                {
                    HandleCutOff(otherLine);                    
                    HandleKill(line, otherLine);                    
                }
            }
        }

        private static void HandleKill(PlayerLine lineKiller, PlayerLine killedLine)
        {
            killedLine.playerFacade.Die();
        }

        private static void HandleCutOff(PlayerLine otherLine)
        {
            otherLine.playerFacade.CutOff();
        }
    }
}
