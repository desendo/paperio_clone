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
            Container.Bind<PlayerZoneView>().AsSingle();
            Container.Bind<PlayerZoneService>().AsSingle();


            Container.BindInterfacesTo<BotMoveHandler>().AsSingle();


        }
    }
}