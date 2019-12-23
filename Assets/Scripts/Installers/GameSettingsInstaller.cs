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
        public PlayerRunner.Settings playerRunnerSettings;
        public PlayerZone.Settings playerZoneSettings;
        public PlayerLine.Settings playerLineSettings;

        
        public override void InstallBindings()
        {
            Container.BindInstance(playerSettings);
            Container.BindInstance(gameInstallerSettings);     
            Container.BindInstance(playerRunnerSettings);     
            Container.BindInstance (playerZoneSettings);     
            Container.BindInstance(playerLineSettings);     

        }
    }
}