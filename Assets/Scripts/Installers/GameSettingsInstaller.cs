using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace Game
{

    //[CreateAssetMenu(menuName = "PaperioClone/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public PlayerMoveHandler.Settings playerMoveHandlerSettings;
        public PlayerRunner.Settings playerRunnerSettins;
        public GameInstaller.Settings gameInstallerSettings;
        public PlayerZone.Settings playerZoneSettings;
        public PlayerZoneView.Settings playerZoneViewSettings;
        public PlayerLine.Settings playerLineSettings;
        public AISettings aISettings;
        public GameController.Settings gameControllerSetting;
        public World.Settings worldSettings;
        public DebugSettings debug;

        public override void InstallBindings()
        {
            Container.BindInstance(playerMoveHandlerSettings);
            Container.BindInstance(playerRunnerSettins);
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

            public bool useWASD = false;
        }

        [Serializable]
        public class AISettings
        {
            public float sensorsDefaultDistance;
            public BotAIPreset[] presets;
        }
    }
}