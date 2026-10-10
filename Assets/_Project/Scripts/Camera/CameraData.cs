using Cinemachine;
using UnityEngine;

namespace FPS
{
    [System.Serializable]
    public class CameraData
    {
        public NoiseSettings CameraNoiseSettings;

        [Min(0.0f)]
        public float CameraNoiseAmplitudeGain = 1.0f;

        [Min(0.0f)]
        public float CameraNoiseFrequencyGain = 1.0f;

        [Min(1)]
        public int cameraFOV = 60;

        [Min(0.1f)]
        public float CameraNominalRotationalVelocity = 720.0f;
    }
}