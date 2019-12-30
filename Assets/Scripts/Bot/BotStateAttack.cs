using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{

    public class BotStateAttack : IBotState
    {
        [Inject] TargetAngleState angleState;
        [Inject] BotSensor botSensor;
        [Inject] BotStateManager stateManager;
        [Inject] PlayerFacade facade;
        [Inject] BotAISessionData session;
        [Inject] GameSettingsInstaller.AISettings aiSettings;
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