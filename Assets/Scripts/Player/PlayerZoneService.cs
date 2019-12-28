using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{
    
    public class PlayerZoneService 
    {
        [Inject]
        PlayerZone zone;
        [Inject]
        PlayerZoneView view;
        [Inject]
        PlayerLine line;
        [Inject]
        CrossingController crossingController;
        [Inject]
        GameSettingsInstaller.DebugSettings debugSettings;
        [Inject]
        Settings _settings;

        [Serializable]
        public class Settings
        {
            public float distanceSimplifiy;

        }
        public PlayerZoneService()
        {
            SignalsController.Default.Add(this);
        }
        ~PlayerZoneService()
        {
            SignalsController.Default.Remove(this);
        }
        public void HandleEnterHomeZone()
        {            
            AddToZone(line.LineDots);           

        }
        private void RemoveFromZone(List<Vector2> line)
        {

            
        }
        private void AddToZone(List<Vector2> line)
        {
            List<Vector2> copyLineNormal = new List<Vector2>();
            List<Vector2> copyLineReversed = new List<Vector2>();
            for (int i = 0; i < line.Count; i++)
            {
                copyLineNormal.Add(line[i]);
                copyLineReversed.Add(line[i]);
            }
            copyLineReversed.Reverse();

            int exitIndex = Helpers.GetNearestBorderPointTo(zone.BorderPoints,line[0]);
            int entryIndex = Helpers.GetNearestBorderPointTo(zone.BorderPoints, line[line.Count-1]);

            //Делаем два куска из изначальной границы
            int splitIndex1 = Mathf.Min(entryIndex, exitIndex);
            int splitIndex2 = Mathf.Max(entryIndex, exitIndex);
            List<Vector2> zonePart1 = new List<Vector2>();
            List<Vector2> zonePart2 = new List<Vector2>();

            List<Vector2> zonePart1_temp = new List<Vector2>();

            for (int i = 0; i < zone.BorderPoints.Count; i++)
            {
                var point = zone.BorderPoints[i];
                if (i <= splitIndex1)
                {
                    zonePart1.Add(point);
                }
                if (i >= splitIndex1 && i <= splitIndex2)
                {
                    zonePart2.Add(point);
                }
                if (i >= splitIndex2)
                {
                    zonePart1_temp.Add(point);
                }
            }
            zonePart1_temp.AddRange(zonePart1);
            zonePart1 = zonePart1_temp;

            //обрезаем кончики добавочной лении чтоб минимизировать шанс коллизий при построении меша. нужные вершины уже в линиях изначальной границы
            copyLineReversed.RemoveAt(copyLineReversed.Count - 1);
            copyLineReversed.RemoveAt(0);
            copyLineNormal.RemoveAt(copyLineNormal.Count - 1);
            copyLineNormal.RemoveAt(0);

            Helpers.SimplifyPolyline(copyLineNormal, _settings.distanceSimplifiy);

            //выбираем конец какой линии куда приставлять
            if (zonePart2[0] == zone.BorderPoints[exitIndex])
            {
                Helpers.SimplifyPolyline(zonePart2, _settings.distanceSimplifiy);
                zonePart2.AddRange(copyLineReversed);
            }
            else
            {
                Helpers.SimplifyPolyline(zonePart2, _settings.distanceSimplifiy);
                zonePart2.AddRange(copyLineNormal);
            }

            if (zonePart1[0] == zone.BorderPoints[exitIndex])
            {
                Helpers.SimplifyPolyline(zonePart1, _settings.distanceSimplifiy);
                zonePart1.AddRange(copyLineReversed);
            }
            else
            {
                Helpers.SimplifyPolyline(zonePart1, _settings.distanceSimplifiy);
                zonePart1.AddRange(copyLineNormal);
            }
         
            // выбираем кусок который использовать.
            float z1 = Triangulator.Area(zonePart1);
            float z2 = Triangulator.Area(zonePart2);

            if (Mathf.Abs(z1) > Mathf.Abs(z2))
            {                
                zone.SetBorder(zonePart1);
            }
            else
            {
                zone.SetBorder(zonePart2);
            } 
        }


        public void PerfomCut(List<Vector2> line)
        {
            RemoveFromZone(line);
        }
        
    }
}
