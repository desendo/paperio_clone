using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{

    public class BotStateGrow : IBotState
    {
        readonly TargetAngleState angleState;
        readonly BotSensor botSensor;
        readonly BotStateManager stateManager;
        readonly PlayerFacade facade;
        readonly BotAISessionData session;
        readonly GameSettingsInstaller.AISettings aiSettings;


        public BotStateGrow(
            TargetAngleState angleState,
            BotSensor botSensor,
            BotStateManager stateManager,
            PlayerFacade facade,
            BotAISessionData session,
            GameSettingsInstaller.AISettings aiSettings)
        {
            this.angleState = angleState;
            this.botSensor = botSensor;
            this.stateManager = stateManager;
            this.facade = facade;
            this.session = session;
            this.aiSettings = aiSettings;
        }

        public void EnterState()
        {
           // Debug.Log("grow "+facade.preset.name);

            session.distanceTraveled = 0f;            
            session.lastPosition = facade.Position;            
        }

        public void ExitState()
        {
        }

        public void FixedUpdate()
        {
        }

        
        public void Update()
        {

            session.discretTimer += Time.deltaTime;
            if (session.discretTimer > aiSettings.aiCalculationsDeltaTime)
            {
                session.discretTimer = 0;
                PerformAiCalc();
            }
        }

        private void PerformAiCalc()
        {
            if (!facade.Inside)
            {
                session.distanceTraveled += (session.lastPosition - (Vector2)facade.Position).magnitude;
                session.lastPosition = facade.Position;
            }
            if (session.distanceTraveled > Random.Range(facade.preset.minimumDistanceToTravel, facade.preset.maximumDistanceToTravel))
            {
                stateManager.ChangeState(BotState.Retreat);

            }

        }
    }
}