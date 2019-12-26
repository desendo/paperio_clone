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

            Rect rect1 = new Rect();
            Rect rect2 = new Rect();
            rect1.InitWithPosition(Vector2.zero);
            rect1.UpdateWithPosition(Vector2.one);

            rect2.InitWithPosition(Vector2.one*1.001f);
            rect2.UpdateWithPosition(Vector2.one * 2f);

            //Debug.Log("overlaps "+rect2.Overlaps(rect1));

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

