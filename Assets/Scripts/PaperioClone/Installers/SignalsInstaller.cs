using Zenject;

namespace PaperIOClone.Installers
{
    public class SignalsInstaller : Installer<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<SignalDie>();
            Container.DeclareSignal<SignalZoneChanged>();
        }
    }
}