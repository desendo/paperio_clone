﻿using System;
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
        int homeEntry;
        int homeExit;
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

            line.Reverse();

            int lineCount = line.Count;
            if (lineCount < 2) return;

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


            if (Mathf.Abs(z1) > Mathf.Abs(z2))
                zone.SetBorder(zonePart1);
            else
                zone.SetBorder(zonePart2);

            Debug.Log("after "+Triangulator.Area(zone.BorderPoints));

            //Helpers.SimplifyPolyline(zone.BorderPoints, _settings.distanceSimplifiy);
            view.UpdateMesh();

            
        }
        List<GameObject> border = new List<GameObject>();
        private void AddToZone(List<Vector2> line)
        {

            for (int i = 0; i < border.Count; i++)
            {
                GameObject.Destroy(border[i]);
            }

            for (int i = 0; i < line.Count; i++)
            {
                var g = Helpers.PlaceCube(line[i], zone.Facade.MainColor, debugSettings.digitCubePrefab, i.ToString());
                g.name = "line " + i;
                border.Add(g);
            }

            for (int i = 0; i < zone.BorderPoints.Count; i++)
            {
                var g = Helpers.PlaceCube(zone.BorderPoints[i], Color.white, debugSettings.digitCubePrefab, i.ToString());
                g.name = "border " + i;
                border.Add(g);
            }

            Debug.Break();
           // return;
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

            homeExit = Helpers.GetNearestBorderPointTo(zone.BorderPoints,line[0]);
            homeEntry = Helpers.GetNearestBorderPointTo(zone.BorderPoints, line[line.Count-1]);

            Debug.Log("home entry " + homeEntry);
            Debug.Log("home exit " + homeExit);
            int i1 = Mathf.Min( homeEntry, homeExit);
            int i2 = Mathf.Max(homeEntry, homeExit);
            
            List<Vector2> zonePart2_1 = new List<Vector2>();
            List<Vector2> zonePart2_2 = new List<Vector2>();

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

            Helpers.SimplifyPolyline(zonePart1, _settings.distanceSimplifiy);
            Helpers.SimplifyPolyline(zonePart2, _settings.distanceSimplifiy);

            if (Mathf.Abs(z1) > Mathf.Abs(z2))
            {
                zone.SetBorder(zonePart1);
            }
            else
            {
                zone.SetBorder(zonePart2);

            }            
            Helpers.SimplifyPolyline(zone.BorderPoints, _settings.distanceSimplifiy);

 



        }


        public void PerfomCut(List<Vector2> line)
        {
            RemoveFromZone(line);
        }
        
    }
}
