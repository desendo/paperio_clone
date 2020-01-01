using System;
using PaperIOClone.Player;
using PaperIOClone.Player.Bot;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Installers
{
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public AISettings aISettings;
        public DebugSettings debug;
        public GameController.Settings gameControllerSetting;
        public GameInstaller.Settings gameInstallerSettings;
        public PlayerLine.Settings playerLineSettings;
        public PlayerMoveHandler.Settings playerMoveHandlerSettings;
        public PlayerRunner.Settings playerRunnerSettings;
        public PlayerZone.Settings playerZoneSettings;
        public PlayerZoneView.Settings playerZoneViewSettings;
        public World.Settings worldSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(playerMoveHandlerSettings);
            Container.BindInstance(playerRunnerSettings);
            Container.BindInstance(gameInstallerSettings);
            Container.BindInstance(gameControllerSetting);
            Container.BindInstance(worldSettings);
            Container.BindInstance(playerZoneSettings);
            Container.BindInstance(playerZoneViewSettings);
            Container.BindInstance(playerLineSettings);
            Container.BindInstance(aISettings);
            Container.BindInstance(debug);
        }

        [Serializable]
        public class DebugSettings
        {
            public Material debugMaterial;
            public GameObject digitCubePrefab;
            public GameObject digitCylPb;
            public bool useWASD;
        }

        [Serializable]
        public class AISettings
        {
            public float aiCalculationsDeltaTime;
            public int defaultSensorsCount;
            public BotAiPreset[] presets;
            public float sensorsDefaultMaxDistance;
            public float sensorsDefaultSteps;
        }
    }
}