using PaperIOClone.Player;
using Zenject;

namespace PaperIOClone.Installers
{
    public class PlayerInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerRunner>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerLine>().AsSingle();
            Container.Bind<PlayerZone>().AsSingle();
            Container.Bind<PlayerZoneService>().AsSingle();
            Container.Bind<InputState>().AsSingle();
            Container.BindInterfacesTo<InputHandler>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();
        }
    }
}