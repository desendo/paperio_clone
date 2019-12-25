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
        PlayerRunner runner;
        [Inject]
        PlayerLine line;

        public PlayerZoneService()
        {
            SignalsController.Default.Add(this);
        }
        ~PlayerZoneService()
        {
            SignalsController.Default.Remove(this);
        }
        internal void ExitHomeZone()
        {

            //    Debug.Log("ExitHomeZone ");

        }

        internal void EnterHomeZone()
        {

            if (line.LineDots.Count < 2) return;
                 Add(line.LineDots);

        }
        public void Add(List<Vector2> line)
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
            if (Mathf.Abs(z1) > Mathf.Abs(z2)) 
                zone.BorderPoints = zonePart1;            
            else 
                zone.BorderPoints = zonePart2;

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
                    ExitHomeZone();
                }
                else
                {
                    homeEntry = arg.nearestBorderPointIndex;
                    EnterHomeZone();
                }
            }

        }
    }
}
