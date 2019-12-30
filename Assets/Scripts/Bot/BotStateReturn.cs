using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class BotStateRetreat : IBotState
    {
        [Inject] TargetAngleState angleState;
        [Inject] BotSensor botSensor;
        [Inject] BotStateManager stateManager;
        [Inject] PlayerFacade facade;
        [Inject] BotAISessionData session;
        [Inject] GameSettingsInstaller.AISettings aiSettings;

        public void EnterState()
        {
            Debug.Log("retreat " + facade.preset.name);
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
            
            Vector2 homeDir = facade.LastHomePosition - (Vector2)facade.Position;

            Debug.DrawLine(facade.Position, (Vector2)facade.Position + homeDir, Color.black);
            float angle = Vector2.Angle(homeDir, facade.LookDir)+UnityEngine.Random.Range(-15f,15f);
            angleState.angle = facade.Rotation+angle*0.5f;

            if(facade.Inside)
                stateManager.ChangeState(BotState.Grow);


        }
    }
}