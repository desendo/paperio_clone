using System;
using UnityEngine;
using Zenject;


namespace Game
{
    public class BotInstaller : Installer
    {

        public override void InstallBindings()
        {

            //Container.Bind<Player>().AsSingle().WithArguments(_settings.rb, _settings.MeshRenderer, _settings.transform);
            //Container.BindInterfacesAndSelfTo<PlayerRunner>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRunner>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerLine>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerZone>().AsSingle();
            Container.Bind<PlayerZoneService>().AsSingle();


            Container.Bind<TargetAngleState>().AsSingle();
            Container.BindInterfacesTo<BotMoveHandler>().AsSingle();

            Container.BindInterfacesTo<BotStateManager>().AsSingle();
            Container.Bind<BotStateGrow>().AsSingle();
            Container.Bind<BotStateAttack>().AsSingle();
            Container.Bind<BotStateRetreat>().AsSingle();

            Container.Bind<BotSensor>().AsSingle();


        }
    }
}