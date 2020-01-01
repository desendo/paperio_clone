using UnityEngine;

namespace PaperIOClone.Player.Bot
{
    [CreateAssetMenu(fileName = "bot AI preset")]
    public class BotAiPreset : ScriptableObject
    {
        public string presetName;
        public float minimumDistanceToTravel;
        public float maximumDistanceToTravel;
        
        public float maximumDistanceFromRoot;
        public float minDistanceToAttack;
        public float maxDistanceToAttack;

        public bool overrideDefaultSensorsParams;
        public float sensorLength;        
        public int sensorCount;


    }
}