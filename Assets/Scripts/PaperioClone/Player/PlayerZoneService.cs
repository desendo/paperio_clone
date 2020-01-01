using System.Collections.Generic;
using PaperIOClone.Helpers;
using PaperIOClone.Installers;
using UnityEngine;

namespace PaperIOClone.Player
{
    public class PlayerZoneService
    {
        private readonly GameSettingsInstaller.DebugSettings _debugSettings;
        private readonly PlayerLine _line;
        private readonly PlayerZone _zone;

        public PlayerZoneService(PlayerZone zone, PlayerLine line, GameSettingsInstaller.DebugSettings debugSettings)
        {
            _zone = zone;
            _line = line;
            _debugSettings = debugSettings;
        }

        public void HandleEnterHomeZone()
        {
            AddLineToZone(_line.Points);
        }

        private void AddLineToZone(List<Vector2> line)
        {
            // в алгоритме специально оставлены избыточные листы для лучше читаемости и его переиспользования. 
            // в продакшон версии от них можно избавится

            //создаем два варианта линии
            var copyLineNormal = new List<Vector2>();
            var copyLineReversed = new List<Vector2>();
            for (var i = 0; i < line.Count; i++)
            {
                copyLineNormal.Add(line[i]);
                copyLineReversed.Add(line[i]);
            }

            copyLineReversed.Reverse();

            var exitIndex = Geometry.GetNearestBorderPointTo(_zone.BorderPoints, line[0]);
            var entryIndex = Geometry.GetNearestBorderPointTo(_zone.BorderPoints, line[line.Count - 1]);

            //Делаем два куска из изначальной границы
            var splitIndex1 = Mathf.Min(entryIndex, exitIndex);
            var splitIndex2 = Mathf.Max(entryIndex, exitIndex);
            var zonePart1 = new List<Vector2>();
            var zonePart2 = new List<Vector2>();

            var zonePart1Temp = new List<Vector2>();

            for (var i = 0; i < _zone.BorderPoints.Count; i++)
            {
                var point = _zone.BorderPoints[i];
                if (i <= splitIndex1) zonePart1.Add(point);
                if (i >= splitIndex1 && i <= splitIndex2) zonePart2.Add(point);
                if (i >= splitIndex2) zonePart1Temp.Add(point);
            }

            zonePart1Temp.AddRange(zonePart1);
            zonePart1 = zonePart1Temp;

            //обрезаем кончики добавочной лении чтоб минимизировать шанс коллизий при построении меша. нужные вершины уже в линиях изначальной границы
            if (line.Count > 2)
            {
                copyLineReversed.RemoveAt(copyLineReversed.Count - 1);
                copyLineReversed.RemoveAt(0);
                copyLineNormal.RemoveAt(copyLineNormal.Count - 1);
                copyLineNormal.RemoveAt(0);
            }
            else
            {
                return; //если участок маленький для текущей частоты добавления точек, то мы его не учитываем
            }

            //выбираем конец какой линии куда приставлять
            zonePart2.AddRange(zonePart2[0] == _zone.BorderPoints[exitIndex] ? copyLineReversed : copyLineNormal);
            zonePart1.AddRange(zonePart1[0] == _zone.BorderPoints[exitIndex] ? copyLineReversed : copyLineNormal);

            // выбираем кусок который использовать. однозначно это тот кусок который больше.
            var z1 = Triangulator.Area(zonePart1);
            var z2 = Triangulator.Area(zonePart2);

            _zone.SetBorder(Mathf.Abs(z1) > Mathf.Abs(z2) ? zonePart1 : zonePart2);
        }
    }
}