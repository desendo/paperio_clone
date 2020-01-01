using PaperIOClone.Installers;

namespace PaperIOClone.Player.Bot.States
{
    public class BotStateAttack : IBotState
    {
        readonly GameSettingsInstaller.AISettings _aiSettings;
        readonly TargetAngleState _angleState;
        readonly BotSensor _botSensor;
        readonly PlayerFacade _facade;
        readonly BotAiSessionData _session;
        readonly BotStateManager _stateManager;

        public BotStateAttack(GameSettingsInstaller.AISettings aiSettings, TargetAngleState angleState, BotSensor botSensor, PlayerFacade facade, BotAiSessionData session, BotStateManager stateManager)
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