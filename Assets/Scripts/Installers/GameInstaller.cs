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

            Container.BindFactory<Vector3, Color, string, PlayerFacade, PlayerFacade.PlayerFactory>()
                .FromPoolableMemoryPool<Vector3, Color,string, PlayerFacade, PlayerFacadePool>(poolBinder => poolBinder
                    .WithInitialSize(1)
                    .FromSubContainerResolve()
                    .ByNewPrefabInstaller<PlayerInstaller>(_settings.playerPrefab)
                    .UnderTransformGroup("ControlablePlayers"));
            
            Container.BindFactory<Vector3, Color, string, PlayerFacade, PlayerFacade.BotFactory>()
                .FromPoolableMemoryPool<Vector3, Color, string, PlayerFacade, BotFacadePool>(poolBinder => poolBinder
                    .WithInitialSize(5)
                    .FromSubContainerResolve()
                    .ByNewPrefabInstaller<BotInstaller>(_settings.playerPrefab)
                    .UnderTransformGroup("Bots"));            

            Container.BindInterfacesAndSelfTo<ControlablePlayerSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<BotSpawner>().AsSingle();
            Container.Bind<CrossingController>().AsSingle();
            Container.Bind<PlayersRegistry>().AsSingle();


        }
        [Serializable]
        public class Settings
        {
            public GameObject playerPrefab;
            public float worldRadius;
        }
        class PlayerFacadePool : MonoPoolableMemoryPool<Vector3, Color, string, IMemoryPool, PlayerFacade>
        {
        }
        class BotFacadePool : MonoPoolableMemoryPool<Vector3, Color, string, IMemoryPool, PlayerFacade>
        {
        }
    }
}

