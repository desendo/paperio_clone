﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{


    public class BotSpawner
    {
        [Inject]
        GameSettingsInstaller.AISettings aISettings;
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
        readonly PlayerFacade.BotFactory _botFactor;

        public BotSpawner(PlayerFacade.BotFactory playerFactory) 
        {
            _botFactor = playerFactory;
        }

        public void SpawnBot(Vector3 pos)
        {
            int randomAIPresetIndex = UnityEngine.Random.Range(0, aISettings.presets.Length);
            var player = _botFactor.Create(pos, Helpers.GetRandomColor(), GetRandomName(), aISettings.presets[randomAIPresetIndex]);
        }

    }
}