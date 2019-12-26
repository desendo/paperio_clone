using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    

    public class BotSpawner
    {
        private static List<string> names = new List<string>()
        {
            "Cybernated Machine",
            "Exploration Device",
            "Responsive Golem",
            "Extra-Terrestrial Emulator",
            "Boomer",
            "Ratcher",
            "Beta",
            "Earl",
            "urix",
            "uzu-01",
            "Spudnik",
            "Scrap",
            "oxeoid",
            "eguroid"

        };
        public static string GetRandomName()
        {
            int randomNameIndex = Random.Range(0, names.Count);
            return names[randomNameIndex];
        }
        readonly PlayerFacade.BotFactory _playerFactory;

        public BotSpawner(PlayerFacade.BotFactory playerFactory) 
        {
            _playerFactory = playerFactory;
        }

        public void SpawnBot(Vector3 pos)
        {
            var player = _playerFactory.Create(pos, Helpers.GetRandomColor(), GetRandomName());
        }

    }
}