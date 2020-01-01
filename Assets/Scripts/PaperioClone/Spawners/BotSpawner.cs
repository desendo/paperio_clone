using System.Collections.Generic;
using PaperIOClone.Installers;
using PaperIOClone.Player;
using UnityEngine;

namespace PaperIOClone.Spawners
{
    public class BotSpawner
    {
        private static readonly List<string> Names = new List<string>
        {
            "Machine",
            "Device",
            "Golem",
            "Emulator",
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

        private readonly GameSettingsInstaller.AISettings _aISettings;
        private readonly PlayerFacade.BotFactory _botFactor;

        public BotSpawner(PlayerFacade.BotFactory playerFactory, GameSettingsInstaller.AISettings aISettings)
        {
            _botFactor = playerFactory;
            _aISettings = aISettings;
        }

        private static string GetRandomName()
        {
            var randomNameIndex = Random.Range(0, Names.Count);
            return Names[randomNameIndex];
        }

        public void SpawnBot(Vector3 pos)
        {
            var randomAiPresetIndex = Random.Range(0, _aISettings.presets.Length);
            var player = _botFactor.Create(pos, Helpers.Geometry.GetRandomColor(), GetRandomName(),
                _aISettings.presets[randomAiPresetIndex]);
        }
    }
}