using Cinemachine;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraStateController : MonoBehaviour
    {
        #region UNITY INSPECTOR FIELDS

        // ============================================================================================
        //  UNITY INSPECTOR FIELDS
        // ============================================================================================

        [Header("Camera Parameters")]

        [SerializeField]
        private FPS.PlayerCameraRootController _cameraControllerComponent;

        [Min(0.0f)]
        [SerializeField]
        private float _cameraFieldOfViewBlend = 10.0f;

        #endregion

        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private CinemachineVirtualCamera _cinemachineVirtualCameraComponent;

        private CinemachineBasicMultiChannelPerlin _cinemachineNoiseComponent;

        private float _currentCameraFieldOfView;

        #endregion

        #region PRIVATE METHODS

        // ============================================================================================
        //  PRIVATE METHODS
        // ============================================================================================

        private void UpdateNoiseProfile()
        {   
            if (_cameraControllerComponent != null)
            {
                FPS.CameraData cameraSettingsProfile = _cameraControllerComponent.CameraSettingsProfile;

                if (cameraSettingsProfile != null)
                {
                    float targetCameraFieldOfView = cameraSettingsProfile.cameraFOV;

                    _currentCameraFieldOfView = Mathf.Lerp(_currentCameraFieldOfView, targetCameraFieldOfView, Time.deltaTime * _cameraFieldOfViewBlend);

                    _cinemachineVirtualCameraComponent.m_Lens.FieldOfView = _currentCameraFieldOfView;
                }
            }
        }

        #endregion

        #region UNITY MONOBEHAVIOUR METHODS

        // ============================================================================================
        //  UNITY MONOBEHAVIOUR METHODS
        // ============================================================================================

        private void Awake()
        {
            _cinemachineVirtualCameraComponent = GetComponent<CinemachineVirtualCamera>();

            _cinemachineNoiseComponent = _cinemachineVirtualCameraComponent.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Update()
        {
            UpdateNoiseProfile();
        }

        #endregion
    }
}
