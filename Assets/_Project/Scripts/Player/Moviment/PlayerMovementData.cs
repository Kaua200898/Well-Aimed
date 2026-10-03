using UnityEngine;

namespace FPS
{
    [System.Serializable]
    public class PlayerMovementData
    {
        [Min(0.1f)]
        public float NominalVelocity = 10.0f;

        [Min(0.1f)]
        public float ForwardMultiplier = 1.00f;

        [Min(0.1f)]
        public float RightMultiplier = 0.75f;

        [Min(0.1f)]
        public float LeftMultiplier = 0.75f;

        [Min(0.1f)]
        public float BackMultiplier = 0.50f;
    }
}
