using System.Collections.Generic;
using PaperIOClone.Installers;
using PaperIOClone.Player;
using ProceduralToolkit;
using ProceduralToolkit.ClipperLib;
using UnityEngine;
using Zenject;
using Geometry = PaperIOClone.Helpers.Geometry;

namespace PaperIOClone
{
    public class CrossingController
    {
        private readonly GameSettingsInstaller.DebugSettings _debugSettings;
        private readonly PlayersRegistry _playersRegistry;
        private readonly SignalBus _signalBus;

        public CrossingController(PlayersRegistry playersRegistry, GameSettingsInstaller.DebugSettings debugSettings,
            SignalBus signalBus)
        {
            _playersRegistry = playersRegistry;
            _debugSettings = debugSettings;
            _signalBus = signalBus;
        }


        public void CheckLineCrossings(Vector2 oldPos, Vector2 pos, PlayerFacade playerFacade)
        {
            HandleLineCrossings(oldPos, pos, playerFacade);
        }

        public void PerformCuts(PlayerZone zone)
        {
            foreach (var zoneToCut in _playersRegistry.Zones)
            {
                if (zoneToCut == zone) continue; // свою зону не отрезаем
                var overlaps =
                    zone.Rect.Overlaps(zoneToCut
                        .Rect); //быстрая проверка а примерно пересекаются ли они, надо ли оно вообще нам.
                if (overlaps) IntersectZones(zone, zoneToCut);
            }
        }

        private void IntersectZones(PlayerZone cuttingZone, PlayerZone zoneToCut)
        {
            var output = GetCrossPolygons(cuttingZone, zoneToCut);

            foreach (var border in output)
            {
                var playerRootPoint = zoneToCut.Facade.LastHomePosition;

                if (zoneToCut.Facade.InsideHome) playerRootPoint = zoneToCut.Facade.Position;

                if (!Geometry.CheckIfInPolygon(border, playerRootPoint)) continue;

                zoneToCut.SetBorder(border);
                zoneToCut.View.UpdateMesh();
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
            var segment = new[] {oldPosition, position};
            var linesArray = _playersRegistry.Lines.ToArray();
            foreach (var otherLine in linesArray)
            {
                if (otherLine.Facade.InsideHome) continue;
                if (Geometry.SegmentCrossesPolyline(
                    segment,
                    otherLine.Points,
                    out var crossing))
                    HandleCutOff(player, otherLine);
            }
        }

        private void HandleCutOff(PlayerFacade killer, PlayerLine killedLine)
        {
            _signalBus.Fire(new SignalDie {killer = killer, victim = killedLine.Facade});
            killedLine.Facade.Die();

            if (killedLine.Facade != killer)
                killer.OnKill();
        }
    }
}