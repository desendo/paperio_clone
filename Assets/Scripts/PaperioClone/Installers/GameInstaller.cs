using System;
using PaperIOClone.Player;
using PaperIOClone.Player.Bot;
using PaperIOClone.Spawners;
using UnityEngine;
using Zenject;

namespace PaperIOClone.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] private readonly Settings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            

            Container.BindFactory<Vector3, Color, string, PlayerFacade, PlayerFacade.PlayerFactory>()
                .FromPoolableMemoryPool<Vector3, Color, string, PlayerFacade, PlayerFacadePool>(poolBinder => poolBinder
                    .WithInitialSize(1)
                    .FromSubContainerResolve()
                    .ByNewPrefabInstaller<PlayerInstaller>(_settings.playerPrefab)
                    .UnderTransformGroup("ControlablePlayers"));

            Container.BindFactory<Vector3, Color, string, BotAiPreset, PlayerFacade, PlayerFacade.BotFactory>()
                .FromPoolableMemoryPool<Vector3, Color, string, BotAiPreset, PlayerFacade, BotFacadePool>(poolBinder =>
                    poolBinder
                        .WithInitialSize(0)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<BotInstaller>(_settings.playerPrefab)
                        .UnderTransformGroup("Bots"));

            Container.BindInterfacesAndSelfTo<ControlablePlayerSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<BotSpawner>().AsSingle();
            Container.Bind<CrossingController>().AsSingle();
            Container.Bind<PlayersRegistry>().AsSingle();
            Container.Bind<ScoresHandler>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<World>().AsSingle();
            
            SignalsInstaller.Install(Container);
        }

        [Serializable]
        public class Settings
        {
            public GameObject playerPrefab;
            public float worldRadius;
        }

        private class PlayerFacadePool : MonoPoolableMemoryPool<Vector3, Color, string, IMemoryPool, PlayerFacade>
        {
        }

        private class
            BotFacadePool : MonoPoolableMemoryPool<Vector3, Color, string, BotAiPreset, IMemoryPool, PlayerFacade>
        {
        }
    }
}