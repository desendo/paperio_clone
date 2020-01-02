using PaperIOClone.Installers;

namespace PaperIOClone.Player.Bot.States
{
    public class BotStateAttack : IBotState
    {
        private readonly GameSettingsInstaller.AISettings _aiSettings;
        private readonly TargetAngleState _angleState;
        private readonly BotSensor _botSensor;
        private readonly PlayerFacade _facade;
        private readonly BotAiSessionData _session;
        private readonly BotStateManager _stateManager;

        public BotStateAttack(GameSettingsInstaller.AISettings aiSettings, TargetAngleState angleState,
            BotSensor botSensor, PlayerFacade facade, BotAiSessionData session, BotStateManager stateManager)
        {
            _aiSettings = aiSettings;
            _angleState = angleState;
            _botSensor = botSensor;
            _facade = facade;
            _session = session;
            _stateManager = stateManager;
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Update()
        {
        }
    }
}