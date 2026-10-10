using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(FPS.PlayerInputController))]
    [RequireComponent(typeof(FPS.PlayerMovementController))]
    public class PlayerCameraRootController : MonoBehaviour
    {
        #region UNITY INSPECTOR FIELDS

        // ============================================================================================
        //  UNITY INSPECTOR FIELDS
        // ============================================================================================

        [Header("Camera Controls")]

        [SerializeField]
        private bool _enableCamera = true;

        [SerializeField]
        private bool _lockPlayerCursor = true;

        [Header("Camera Yaw")]

        [SerializeField]
        private bool _invertYawAxisControls = false;

        [Header("Camera Pitch")]

        [SerializeField]
        private bool _invertPitchAxisControls = false;

        [Range(0.0f, 180.0f)]
        [SerializeField]
        private float _maximumPitchAngle = 90.0f;

        [Range(-180.0f, 0.0f)]
        [SerializeField]
        private float _minimumPitchAngle = -90.0f;

        [Header("Camera Sensitivity")]

        [Range(0.1f, 1.0f)]
        [SerializeField]
        private float _xAxisSensitivity = 0.5f;

        [Range(0.1f, 1.0f)]
        [SerializeField]
        private float _yAxisSensitivity = 0.5f;

        [Header("Camera Movement")]

        [SerializeField]
        private Transform _cameraRootReference;

        [SerializeField]
        private Transform _cameraAnchorReference;

        [Header("Camera Settings Profiles")]

        [SerializeField]
        private FPS.CameraData _idleCameraSettingsProfile = new FPS.CameraData()
        {
            CameraNominalRotationalVelocity = 2.0f
        };

        [SerializeField]
        private FPS.CameraData _walkingCameraSettingsProfile = new FPS.CameraData()
        {
            CameraNominalRotationalVelocity = 2.0f
        };

        [SerializeField]
        private FPS.CameraData _sprintingCameraSettingsProfile = new FPS.CameraData()
        {
            CameraNominalRotationalVelocity = 2.0f
        };

        [SerializeField]
        private FPS.CameraData _aimCameraSettingsProfile = new FPS.CameraData()
        {
            CameraNominalRotationalVelocity = 0.5f
        };

        [SerializeField]
        private FPS.CameraData _airbourneCameraSettingsProfile = new FPS.CameraData()
        {
            CameraNominalRotationalVelocity = 2.0f
        };

        #endregion

        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private FPS.PlayerInputController _soldierInputControllerComponent;

        private FPS.PlayerMovementController _soldierMovementControllerComponent;

        private FPS.CameraData _cameraSettingsProfile;

        private float _currentYaw = 0.0f;

        private float _currentPitch = 0.0f;

        #endregion

        #region PUBLIC MEMBERS

        // ============================================================================================
        //  PUBLIC MEMBERS
        // ============================================================================================

        public FPS.CameraData CameraSettingsProfile => _cameraSettingsProfile;

        public float CurrentYaw => _currentYaw;

        public float CurrentPitch => _currentPitch;

        #endregion

        #region PRIVATE METHODS

        // ============================================================================================
        //  PRIVATE METHODS
        // ============================================================================================

        private void InitializeCameraAngles()
        {
            if (_cameraRootReference != null)
            {
                _currentYaw = _cameraRootReference.eulerAngles.y;
            }

            if (_cameraAnchorReference != null)
            {
                float seedPitch = _cameraAnchorReference.localEulerAngles.x;

                if (seedPitch > 180.0f)
                {
                    seedPitch -= 360.0f;
                }

                _currentPitch = Mathf.Clamp(seedPitch, _minimumPitchAngle, _maximumPitchAngle);
            }
        }

        private void ApplyPlayerCursorLock()
        {
            if (_lockPlayerCursor == true)
            {
                Cursor.lockState = CursorLockMode.Locked;

                Cursor.visible = false;
            }
        }

        private void UpdateCameraSettingsProfile()
        {
            if (_soldierMovementControllerComponent.IsAiming == true)
            {
                _cameraSettingsProfile = _aimCameraSettingsProfile;
            }
            else if (_soldierMovementControllerComponent.IsSprinting)
            {
                _cameraSettingsProfile = _sprintingCameraSettingsProfile;
            }
            else
            {
                switch (_soldierMovementControllerComponent.MovementState)
                {
                    case FPS.EPlayerMovementState.Airborne:

                        _cameraSettingsProfile = _airbourneCameraSettingsProfile;

                        break;

                    case FPS.EPlayerMovementState.Walking:

                        _cameraSettingsProfile = _walkingCameraSettingsProfile;

                        break;

                    case FPS.EPlayerMovementState.Sprinting:

                        _cameraSettingsProfile = _sprintingCameraSettingsProfile;

                        break;

                    case FPS.EPlayerMovementState.Idle:

                    default:

                        _cameraSettingsProfile = _idleCameraSettingsProfile;

                        break;
                }
            }
        }

        private void ApplyRootMovement()
        {
            if (_enableCamera == true && _cameraRootReference != null && _cameraSettingsProfile != null)
            {
                float yawAxis = _soldierInputControllerComponent.LookVector.x;

                if (_invertYawAxisControls == true)
                {
                    yawAxis *= -1;
                }

                float yawDelta = yawAxis * _cameraSettingsProfile.CameraNominalRotationalVelocity * _xAxisSensitivity;

                _currentYaw += yawDelta;

                _cameraRootReference.rotation = Quaternion.Euler(0.0f, _currentYaw, 0.0f);
            }
        }

        private void ApplyAnchorMovement()
        {
            if (_enableCamera == true && _cameraAnchorReference != null && _cameraSettingsProfile != null)
            {
                float pitchAxis = _soldierInputControllerComponent.LookVector.y;

                if (_invertPitchAxisControls == false)
                {
                    pitchAxis *= -1;
                }

                float pitchDelta = pitchAxis * _cameraSettingsProfile.CameraNominalRotationalVelocity * _yAxisSensitivity;

                _currentPitch = Mathf.Clamp(_currentPitch + pitchDelta, _minimumPitchAngle, _maximumPitchAngle);

                _cameraAnchorReference.localRotation = Quaternion.Euler(_currentPitch, 0.0f, 0.0f);
            }
        }

        #endregion

        #region UNITY MONOBEHAVIOUR METHODS

        // ============================================================================================
        //  UNITY MONOBEHAVIOUR METHODS
        // ============================================================================================

        private void Awake()
        {
            _soldierInputControllerComponent = GetComponent<FPS.PlayerInputController>();

            _soldierMovementControllerComponent = GetComponent<FPS.PlayerMovementController>();

            InitializeCameraAngles();
        }

        private void Start()
        {
            ApplyPlayerCursorLock();
        }

        private void LateUpdate()
        {
            UpdateCameraSettingsProfile();

            ApplyRootMovement();

            ApplyAnchorMovement();
        }

        #endregion
    }
}
