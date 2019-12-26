using System;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerInstaller : Installer
    {

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerRunner>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerLine>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerZone>().AsSingle();            
            Container.Bind<PlayerZoneView>().AsSingle();
            Container.Bind<PlayerZoneService>().AsSingle();
            
            Container.Bind<InputState>().AsSingle();
            Container.BindInterfacesTo<InputHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();
            
        }
    }
}