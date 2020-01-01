using PaperIOClone.Installers;

namespace PaperIOClone.Player.Bot
{
    public class BotSensor
    {
        private readonly GameSettingsInstaller.AISettings _aISettings;
        private readonly PlayerFacade _playerFacade;

        public BotSensor(GameSettingsInstaller.AISettings aISettings, PlayerFacade playerFacade)
        {
            _aISettings = aISettings;
            _playerFacade = playerFacade;
        }

        public void CheckSensorsData()
        {
            var sensorsCount = _aISettings.defaultSensorsCount;
            var angleStep = 360f / sensorsCount;
        }
    }
}