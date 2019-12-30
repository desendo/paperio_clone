using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    [CreateAssetMenu(fileName = "bot AI preset")]
    public class BotAIPreset : ScriptableObject
    {
        public string presetName;
        public float minimumDistanceToTravel;
        public float maximumDistanceToTravel;
        
        public float maximumDistanceFromRoot;
        public float minDistanceToAttack;
        public float maxDistanceToAttack;

        public bool overrideDefaultSensorsParams;
        public float sensorLenght;        
        public int sensorCount;


    }
}