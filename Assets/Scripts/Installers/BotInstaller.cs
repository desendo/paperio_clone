using System;
using UnityEngine;
using Zenject;


namespace Game
{
    public class BotInstaller : Installer
    {

        public override void InstallBindings()
        {
            Debug.Log("InstallBindings bot");
            //Container.Bind<PlayerRunner>().AsSingle();

            //Container.BindInterfacesTo<InputHandler>().AsSingle();
            //Container.Bind<InputState>().AsSingle();
            // Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();
            Container.Bind<PlayerLine>().AsSingle();
            Container.Bind<PlayerZone>().AsSingle();


        }
    }
}