using System;
using UnityEngine;
using Zenject;

namespace PaperIOClone
{
    public class World : IInitializable
    {
        private readonly Settings _settings;
        private readonly WorldView _worldView;

        public World(Settings settings, WorldView worldView)
        {
            _settings = settings;
            _worldView = worldView;

            Center = Vector2.zero;
        }

        public float Radius => _settings.radius;
        public Vector2 Center { get; }
        public float Area => Radius * Radius * 3.14159265359f;

        public void Initialize()
        {
            _worldView.UpdateView(Center, _settings.radius, _settings.vertexCount);
        }

        [Serializable]
        public class Settings
        {
            public float radius;
            public int vertexCount;
        }
    }
}