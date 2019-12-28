using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System;
using ProceduralToolkit.ClipperLib;
using ProceduralToolkit;

namespace Game
{

    public class CrossingController
    {

        [Inject]
        PlayersRegistry playersRegistry;
        [Inject]
        GameSettingsInstaller.DebugSettings debugSettings;
        public CrossingController(PlayersRegistry playersRegistry)
        {
            this.playersRegistry = playersRegistry;
        }


        public void OnLinePointAdded(PlayerLine line)
        {
            HandleLineCrossings(line);
        }

        public void PerformCuts(PlayerZone zone)
        {
            foreach (var zoneToCut in playersRegistry.Zones)
            {
                if (zoneToCut == zone) continue; // свою зону не отрезаем
                bool overlaps = zone.rect.Overlaps(zoneToCut.rect);
                if (overlaps)
                {
                    ZoneCross(zone, zoneToCut);
                }
            }
        }
        private void ZoneCross(PlayerZone cuttingZone, PlayerZone zoneToCut)
        {
            var output = new List<List<Vector2>>();

            var clipper = new PathClipper();
            clipper.AddPath(zoneToCut.BorderPoints, PolyType.ptSubject);
            clipper.AddPath(cuttingZone.BorderPoints, PolyType.ptClip);
            clipper.Clip(ClipType.ctDifference, ref output);

            for (int i = 0; i < output.Count; i++)
            {
                List<Vector2> border = output[i];

                Vector2 playerRootPoint = zoneToCut.Facade.LastHomePosition;

                if (zoneToCut.Facade.Inside)
                {
                    playerRootPoint = zoneToCut.Facade.Position;
                }

                if (Helpers.CheckIfInPolygon(border, playerRootPoint))
                {
                    zoneToCut.SetBorder(border);
                    zoneToCut.view.UpdateMesh();
                }
                else
                {
                    Debug.Log("not included");
                }

            }

        }
        private void HandleLineCrossings(PlayerLine line)
        {
            int count = line.Points.Count;
            if (count < 2) return;
            var linesArray = playersRegistry.Lines.ToArray();
            foreach (var otherLine in linesArray)
            {
                Vector2 crossing = Vector2.zero;
                if (Helpers.SegmentCrossesPolyline(
                    line.LastSegment,
                    otherLine.Points,
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
