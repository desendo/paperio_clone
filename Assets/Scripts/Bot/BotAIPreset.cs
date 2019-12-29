using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    [CreateAssetMenu(fileName = "bot AI preset")]
    public class BotAIPreset : ScriptableObject
    {
        public float minimumDistanceToTravel;
        public float maximumDistanceToTravel;
        
        public float maximumDistanceFromRoot;
        public float distance;

        public bool overrideSensorLenght;
        public float sensorLenght;

        public bool overrideSensorCount;
        public int sensorCount;


    }
}