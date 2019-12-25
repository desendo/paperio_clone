using System;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        Settings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();


            Container.BindFactory<float, float, PlayerFacade, PlayerFacade.PlayerFactory>()
                .FromPoolableMemoryPool<float, float, PlayerFacade, PlayerFacadePool>(poolBinder => poolBinder
                    .WithInitialSize(1)
                    .FromSubContainerResolve()
                    .ByNewPrefabInstaller<PlayerInstaller>(_settings.playerPrefab)
                    .UnderTransformGroup("ControlablePlayers"));

            /*
            Container.BindFactory<float, float, PlayerFacade, PlayerFacade.BotFactory>()
                .FromPoolableMemoryPool<float, float, PlayerFacade, BotFacadePool>(poolBinder => poolBinder
                    .WithInitialSize(5)
                    .FromSubContainerResolve()
                    .ByNewPrefabInstaller<BotInstaller>(_settings.playerPrefab)
                    .UnderTransformGroup("Bots"));

            */

            Container.BindInterfacesAndSelfTo<ControlablePlayerSpawner>().AsSingle();

            //Container.BindInterfacesAndSelfTo<BotSpawner>().AsSingle();
            Container.Bind<LineCrossingController>().AsSingle();
            Container.Bind<PlayersRegistry>().AsSingle();
            

        }
        [Serializable]
        public class Settings
        {
            public GameObject playerPrefab;
            public float worldRadius;
        }
        class PlayerFacadePool : MonoPoolableMemoryPool<float, float, IMemoryPool, PlayerFacade>
        {
        }
        class BotFacadePool : MonoPoolableMemoryPool<float, float, IMemoryPool, PlayerFacade>
        {
        }
    }
}

