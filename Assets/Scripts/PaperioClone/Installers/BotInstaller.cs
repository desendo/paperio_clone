using PaperIOClone.Player;
using PaperIOClone.Player.Bot;
using PaperIOClone.Player.Bot.States;
using Zenject;

namespace PaperIOClone.Installers
{
    public class BotInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerRunner>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerLine>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerZone>().AsSingle();
            Container.Bind<PlayerZoneService>().AsSingle();

            Container.BindInterfacesAndSelfTo<BotStateManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TargetAngleState>().AsSingle();
            Container.BindInterfacesTo<PlayerMoveHandler>().AsSingle();

            Container.Bind<BotStateGrow>().AsSingle();
            Container.Bind<BotStateAttack>().AsSingle();
            Container.Bind<BotStateRetreat>().AsSingle();

            Container.Bind<BotSensor>().AsSingle();
            Container.Bind<BotAiSessionData>().AsSingle();
        }
    }
}