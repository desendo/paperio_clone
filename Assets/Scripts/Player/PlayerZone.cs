using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerZone : IInitializable
    {
        [Inject]
        Settings settings;
        [Inject]
        PlayerZoneView view;
        [Inject]
        PlayerZoneService service;
        readonly PlayerFacade playerFacade;
        
        private List<Vector2> borderPoints;

        
        public PlayerZone(PlayerFacade playerFacade)
        {
            this.playerFacade = playerFacade;
            borderPoints = new List<Vector2>();
            
        }
        public bool IsInZone(Vector3 position)
        {
            return Helpers.CheckIfInPolygon(BorderPointsArray, position);
        }
        public List<Vector2> BorderPoints
        {
            get => borderPoints;            
            set => borderPoints = value;            
        }
        public Vector2[] BorderPointsArray
        {
            get => borderPoints.ToArray();
        }
        [System.Serializable]
        public class Settings
        {
         
            public float initialRadius;
            public int initialDotsCount;
            public Material debugFirstPoint;
            public Material debugSecondPoint;
            public Material debugOtherPoints;
            
        }
        public int[] GetNearestBorderEdgeTo(Vector3 position)
        {
            int[] pair = new int[2];

            float sqaredDist = float.PositiveInfinity;
            for (int i = 0; i < borderPoints.Count; i++)
            {
                float x1 = borderPoints[i].x;
                float y1 = borderPoints[i].y;

                int next = i+1;
                if (next >= borderPoints.Count)
                    next = 0;

                float x2 = borderPoints[next].x;
                float y2 = borderPoints[next].y;

                float x0 = position.x;
                float y0 = position.y;
                float sqaredDistMinPoint1 = (x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0);
                float sqaredDistMinPoint2 = (x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0);
                if (sqaredDistMinPoint2 + sqaredDistMinPoint1 < sqaredDist)
                {
                    pair[0] = i;
                    pair[1] = next;

                    sqaredDist = sqaredDistMinPoint2 + sqaredDistMinPoint1;
                }

            }

            return pair;
        }
        public int GetNearestBorderPointTo(Vector3 position)
        {
            float sqaredDist = float.PositiveInfinity;
            int indexOfNearest = -1;
            for (int i = 0; i < borderPoints.Count; i++)
            {
                float x1 = borderPoints[i].x;
                float y1 = borderPoints[i].y;

                float x2 = position.x;
                float y2 = position.y;
                float sqaredDistMin = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
                if (sqaredDistMin < sqaredDist)
                {
                    indexOfNearest = i;
                    sqaredDist = sqaredDistMin;
                }

            }
            return indexOfNearest;

        }
        
        public void GenerateCirclePolygon()
        {
            int vertsCount = settings.initialDotsCount;
            float r = settings.initialRadius;
            float step = 360f / vertsCount;
            //float phase = UnityEngine.Random.value * 360f * 3.1415f;
            float phase =0.001f;
            for (int i = 0; i < vertsCount; i++)
            {
                float rad = (i * step) / 180.0f * 3.1415f + phase;
                float x = (r * Mathf.Cos(rad ) );
                float y = (r * Mathf.Sin(rad));
                borderPoints.Add(new Vector2(x, y));
            }            
        }

        List<GameObject> debugSpheres= new List<GameObject>();
        internal void DrawDebugBorder()
        {
            foreach (var item in debugSpheres)
            {
                GameObject.Destroy(item);
            }
            debugSpheres.Clear();
            int i = 0;


            foreach (var p in borderPoints)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.parent = playerFacade.transform;
                go.transform.position = p;
                go.transform.localScale *= 0.5f;
                go.name = "b "+i.ToString();
                if (i == 0)
                    go.GetComponent<MeshRenderer>().material = settings.debugFirstPoint;
                else if (i == 1)
                    go.GetComponent<MeshRenderer>().material = settings.debugSecondPoint;
                else
                    go.GetComponent<MeshRenderer>().material = settings.debugOtherPoints;
                debugSpheres.Add(go);
                i++;
            }
        }

        public void Initialize()
        {

            GenerateCirclePolygon();
            view.Initialize();
            view.UpdateMesh();
        }
    }
}