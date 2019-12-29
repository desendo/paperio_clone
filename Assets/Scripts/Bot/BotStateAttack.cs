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