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
        public PlayerMoveHandler.Settings playerSettings;
        public GameInstaller.Settings gameInstallerSettings;
        public PlayerZone.Settings playerZoneSettings;
        public PlayerZoneView.Settings playerZoneViewSettings;
        public PlayerLine.Settings playerLineSettings;
        public DebugSettings debug;


        public override void InstallBindings()
        {
            Container.BindInstance(playerSettings);
            Container.BindInstance(gameInstallerSettings);
            Container.BindInstance(playerZoneSettings);
            Container.BindInstance(playerZoneViewSettings);
            Container.BindInstance(playerLineSettings);

            Container.BindInstance(debug);

        }

        [Serializable]
        public class DebugSettings
        {
            public Material debugMaterial;
        }
    }
}