using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{

    public class BotStateGrow : IBotState
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
            angleState.angle += Time.deltaTime * 100f;
        }
    }
}