using PaperIOClone.Installers;
using UnityEngine;

namespace PaperIOClone.Player.Bot.States
{
    public class BotStateRetreat : IBotState
    {
        private readonly GameSettingsInstaller.AISettings _aiSettings;
        private readonly TargetAngleState _angleState;
        private readonly BotSensor _botSensor;
        private readonly PlayerFacade _facade;
        private readonly BotAiSessionData _session;
        private readonly BotStateManager _stateManager;


        public BotStateRetreat(GameSettingsInstaller.AISettings aiSettings, TargetAngleState angleState,
            PlayerFacade facade, BotAiSessionData session, BotStateManager stateManager, BotSensor botSensor)
        {
            _aiSettings = aiSettings;
            _angleState = angleState;
            _facade = facade;
            _session = session;
            _stateManager = stateManager;
            _botSensor = botSensor;
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
            _session.DiscreteTimer += Time.deltaTime;
            if (_session.DiscreteTimer > _aiSettings.aiCalculationsDeltaTime)
            {
                _session.DiscreteTimer = 0;
                PerformAiCalc();
            }
        }

        private void PerformAiCalc()
        {
            var homeDir = _facade.LastHomePosition - (Vector2) _facade.Position;
            var angle = Vector2.Angle(homeDir, _facade.LookDir) + Random.Range(-15f, 15f);
            _angleState.Angle = _facade.Rotation + angle * 0.5f;

            if (_facade.InsideHome)
                _stateManager.ChangeState(BotState.Grow);
        }
    }
}