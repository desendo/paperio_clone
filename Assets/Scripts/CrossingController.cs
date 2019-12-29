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


        public void CheckLineCrossings(Vector2 oldPos, Vector2 pos,PlayerFacade playerFacade)
        {
            HandleLineCrossings(oldPos,pos,playerFacade);
        }

        public void PerformCuts(PlayerZone zone)
        {
            foreach (var zoneToCut in playersRegistry.Zones)
            {
                if (zoneToCut == zone) continue; // свою зону не отрезаем
                bool overlaps = zone.rect.Overlaps(zoneToCut.rect); //быстрая проверка а примерно пересекаются ли они, надо ли оно вообще нам.
                if (overlaps)
                {
                    IntersectZones(zone, zoneToCut);
                }
            }
        }
        private void IntersectZones(PlayerZone cuttingZone, PlayerZone zoneToCut)
        {
            List<List<Vector2>> output = GetCrossPolygons(cuttingZone, zoneToCut);

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

        private List<List<Vector2>> GetCrossPolygons(PlayerZone cuttingZone, PlayerZone zoneToCut)
        {
            var output = new List<List<Vector2>>();

            var clipper = new PathClipper();
            clipper.AddPath(zoneToCut.BorderPoints, PolyType.ptSubject);
            clipper.AddPath(cuttingZone.BorderPoints, PolyType.ptClip);
            clipper.Clip(ClipType.ctDifference, ref output);
            return output;
        }

        private void HandleLineCrossings(Vector2 oldPosition, Vector2 position, PlayerFacade player)
        {
            var segment = new Vector2[] { oldPosition, position };
            var linesArray = playersRegistry.Lines.ToArray();
            foreach (var otherLine in linesArray)
            {
                if (otherLine.Facade.Inside) continue;
                if (Helpers.SegmentCrossesPolyline(
                    segment,
                    otherLine.Points,
                    out Vector2 crossing))
                {
                    HandleCutOff(player, otherLine);
                }
            }
        }
        private static void HandleCutOff(PlayerFacade killer, PlayerLine killedLine)
        {
            //todo отсрочку килла как в оригинале
            killedLine.Facade.Die();
            killer.OnKill();
        }


    }
}
