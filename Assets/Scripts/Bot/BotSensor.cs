using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class BotSensor 
    {
        [Inject]
        GameSettingsInstaller.AISettings aISettings;
        [Inject]
        PlayerFacade playerFacade;
        public void CheckSensorsData()
        {
            int sensorsCount = aISettings.defaultSensorsCount;
            float angleStep = 360f / sensorsCount;
            for (int i = 0; i < aISettings.defaultSensorsCount; i++)
            {
                //float sensorAngleDelta = i * angleStep + playerFacade.Rotation;
                
            }
        }
    }
}