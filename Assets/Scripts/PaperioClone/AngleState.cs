using PaperIOClone.Player;
using UnityEngine;

namespace PaperIOClone
{
    public class AngleState : ITargetAngleState
    {
        public Vector2 TotalDelta;
        public float Angle => Mathf.Atan2(TotalDelta.normalized.y, TotalDelta.normalized.x) * Mathf.Rad2Deg;
    }
}