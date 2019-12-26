using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{


    
    public class PlayerZoneService : IReceive<SignalZoneBorderPass>
    {
        [Inject]
        PlayerZone zone;
        [Inject]
        PlayerZoneView view;
        [Inject]
        PlayerLine line;
        [Inject]
        CrossingController crossingController;
        public PlayerZoneService()
        {
            SignalsController.Default.Add(this);
        }
        ~PlayerZoneService()
        {
            SignalsController.Default.Remove(this);
        }
        private void HandleEnterHomeZone()
        {            
            AddToZone(line.LineDots);           

        }
        private void RemoveFromZone(List<Vector2> line)
        { 
            //очень странно но алгоритм совершенно такой же. наверное в математике есть какая нить теорема
            //to do привести оба метода к общему знаменателю
            int lineCount = line.Count;
            

            Vector2 entryPos = line[0];
            Vector2 exitPos = line[lineCount-1];

            int entryIndex = Helpers.GetNearestBorderPointTo(zone.BorderPoints, entryPos);
            int exitIndex = Helpers.GetNearestBorderPointTo(zone.BorderPoints, exitPos);


            bool isBorderClockwise = Triangulator.Area(zone.BorderPoints) < 0;
            bool isLineClockWise = Triangulator.Area(line) < 0;

            List<Vector2> zonePart1 = new List<Vector2>();
            List<Vector2> zonePart2 = new List<Vector2>();

            List<Vector2> copyLineNormal = new List<Vector2>();
            List<Vector2> copyLineReversed = new List<Vector2>();
            for (int i = 0; i < line.Count; i++)
            {
                copyLineNormal.Add(line[i]);
                copyLineReversed.Add(line[i]);
            }
        
            copyLineReversed.Reverse();

            int i1 = Mathf.Min(entryIndex, exitIndex);
            int i2 = Mathf.Max(entryIndex, exitIndex);

            List<Vector2> zonePart2_1 = new List<Vector2>();
            List<Vector2> zonePart2_2 = new List<Vector2>();


            for (int i = 0; i < zone.BorderPoints.Count; i++)
            {
                if (i >= i1 && i <= i2)
                {
                    zonePart1.Add(zone.BorderPoints[i]);
                }
                if (i <= i1)
                {
                    zonePart2_1.Add(zone.BorderPoints[i]);
                }
                if (i >= i2)
                {
                    zonePart2_2.Add(zone.BorderPoints[i]);
                }
            }

            zonePart2_2.AddRange(zonePart2_1);
            zonePart2 = zonePart2_2;

            if (zonePart1[0] == zone.BorderPoints[homeExit])
                zonePart1.AddRange(copyLineReversed);
            else
                zonePart1.AddRange(copyLineNormal);

            if (zonePart2[0] == zone.BorderPoints[homeExit])
                zonePart2.AddRange(copyLineReversed);
            else
                zonePart2.AddRange(copyLineNormal);

            float z1 = Triangulator.Area(zonePart1);
            float z2 = Triangulator.Area(zonePart2);


            if (Mathf.Abs(z1)> Mathf.Abs(z2))
                zone.SetBorder(zonePart1);
            else
                zone.SetBorder(zonePart2);

            view.UpdateMesh();

            
        }

        private void AddToZone(List<Vector2> line)
        {
            bool isBorderClockwise = Triangulator.Area(zone.BorderPoints) < 0;
            bool isLineClockWise = Triangulator.Area(line) < 0;

            List<Vector2> zonePart1 = new List<Vector2>();
            List<Vector2> zonePart2 = new List<Vector2>();

            List<Vector2> copyLineNormal = new List<Vector2>();
            List<Vector2> copyLineReversed = new List<Vector2>();
            for (int i = 0; i < line.Count; i++)
            {
                copyLineNormal.Add(line[i]);
                copyLineReversed.Add(line[i]);
            }

            copyLineReversed.Reverse();

            int i1 = Mathf.Min( homeEntry, homeExit);
            int i2 = Mathf.Max(homeEntry, homeExit);
            
            List<Vector2> zonePart2_1 = new List<Vector2>();
            List<Vector2> zonePart2_2 = new List<Vector2>();
            for (int i = 0; i < zone.BorderPoints.Count; i++)
            {
                if (i >= i1 && i <= i2)
                {
                    zonePart1.Add(zone.BorderPoints[i]);
                }
                if (i <= i1)
                {
                    zonePart2_1.Add(zone.BorderPoints[i]);
                }
                if (i >= i2)
                {
                    zonePart2_2.Add(zone.BorderPoints[i]);
                }
            }

            zonePart2_2.AddRange(zonePart2_1);
            zonePart2 = zonePart2_2;

            if (zonePart1[0] == zone.BorderPoints[homeExit])
                zonePart1.AddRange(copyLineReversed);
            else
                zonePart1.AddRange(copyLineNormal);

            if (zonePart2[0] == zone.BorderPoints[homeExit])            
                zonePart2.AddRange(copyLineReversed);            
            else
                zonePart2.AddRange(copyLineNormal);

            float z1 = Triangulator.Area(zonePart1);
            float z2 = Triangulator.Area(zonePart2);
            //здесь вычисляем по какой границе получится наибольший по площади полигон и применяем его
            if (Mathf.Abs(z1) > Mathf.Abs(z2)) 
                zone.SetBorder(zonePart1);            
            else 
                zone.SetBorder(zonePart2);
        }
        int homeEntry;
        int homeExit;
        public void HandleSignal(SignalZoneBorderPass arg)
        {
            if (arg.zone == zone)
            {
                if (arg.isExiting)
                {
                    homeExit = arg.nearestBorderPointIndex;
                }
                else
                {
                    homeEntry = arg.nearestBorderPointIndex;
                    HandleEnterHomeZone();
                }
            }
        }

        public void PerfomCut(PlayerLine line, int enterIndex, int exitIndex)
        {
            List<Vector2> cutLine = line.LineDots.GetRange(enterIndex, exitIndex-enterIndex-1);
            Debug.Log("cutLine  "+cutLine.Count);

            RemoveFromZone(cutLine);
        }
        
    }
}
