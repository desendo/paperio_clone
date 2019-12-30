using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Game
{


    public class World : IInitializable
    {
        public Settings settings { get; private set; }

        [Inject]
        private WorldView _worldView;
        public float Radius { get => settings.radius; }
        public Vector2 Center { get; private set; }
        public World(Settings settings)
        {
            this.settings = settings;
            Center = Vector2.zero;            
        }

        [System.Serializable]

        public class Settings
        {
            public float radius;
            public int vertexCount;
        }
        public float Area
        {
            get => Radius * Radius * 3.14159265359f;
        }
        public void Initialize()
        {
            _worldView.UpdateView(Center, settings.radius, settings.vertexCount);
        }
    }



      
}