using System;
using UnityEngine;
using Zenject;


namespace Game
{
    public class PlayerInstaller : Installer
    {

        public override void InstallBindings()
        {

            //Container.Bind<Player>().AsSingle().WithArguments(_settings.rb, _settings.MeshRenderer, _settings.transform);
            Container.BindInterfacesTo<InputHandler>().AsSingle();
            Container.Bind<InputState>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();


        }
    }
}