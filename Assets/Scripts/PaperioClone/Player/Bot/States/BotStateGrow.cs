using PaperIOClone.Installers;
using UnityEngine;

namespace PaperIOClone.Player.Bot.States
{
    public class BotStateGrow : IBotState
    {
        private readonly GameSettingsInstaller.AISettings _aiSettings;
        private readonly TargetAngleState _angleState;
        private readonly BotSensor _botSensor;
        private readonly PlayerFacade _facade;
        private readonly BotAiSessionData _session;
        private readonly BotStateManager _stateManager;


        public BotStateGrow(
            TargetAngleState angleState,
            BotSensor botSensor,
            BotStateManager stateManager,
            PlayerFacade facade,
            BotAiSessionData session,
            GameSettingsInstaller.AISettings aiSettings)
        {
            _angleState = angleState;
            _botSensor = botSensor;
            _stateManager = stateManager;
            _facade = facade;
            _session = session;
            _aiSettings = aiSettings;
        }

        public void EnterState()
        {
            _session.DistanceTraveled = 0f;
            _session.LastPosition = _facade.Position;
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
            if (!_facade.InsideHome)
            {
                _session.DistanceTraveled += (_session.LastPosition - (Vector2) _facade.Position).magnitude;
                _session.LastPosition = _facade.Position;
            }

            if (_session.DistanceTraveled > Random.Range(_facade.Preset.minimumDistanceToTravel,
                    _facade.Preset.maximumDistanceToTravel)) _stateManager.ChangeState(BotState.Retreat);
        }
    }
}