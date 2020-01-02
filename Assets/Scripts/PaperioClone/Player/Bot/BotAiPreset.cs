using UnityEngine;

namespace PaperIOClone.Player.Bot
{
    [CreateAssetMenu(fileName = "bot AI preset")]
    public class BotAiPreset : ScriptableObject
    {
        public float maxDistanceToAttack;

        public float maximumDistanceFromRoot;
        public float maximumDistanceToTravel;
        public float minDistanceToAttack;
        public float minimumDistanceToTravel;

        public bool overrideDefaultSensorsParams;
        public string presetName;
        public int sensorCount;
        public float sensorLength;
    }
}