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
        public LineSettings lineSettings;
        public GameInstaller.Settings gameInstallerSettings;

        [Serializable]
        public class CharacterSettings
        {
            public float moveSpeed;
            public float rotateSpeed;

        }

        [Serializable]
        public class LineSettings
        {
            public float discretizationFactor;
            public float destroySpeed;
        }

        
        public override void InstallBindings()
        {
            Container.BindInstance(playerSettings);
            Container.BindInstance(lineSettings);
            Container.BindInstance(gameInstallerSettings);     

        }
    }
}