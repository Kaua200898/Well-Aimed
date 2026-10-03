using System;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FPS.PlayerInputController))]
    public class PlayerMovementController : MonoBehaviour
    {
        #region UNITY INSPECTOR FIELDS

        // ============================================================================================
        //  UNITY INSPECTOR FIELDS
        // ============================================================================================

        [Header("Movement Direction")]

        [SerializeField]
        private Transform _movementDirectionRootReference;

        [Header("Walk Movement")]

        [SerializeField]
        private bool _enableWalk = true;

        [SerializeField]
        private FPS.PlayerMovementData _walkVelocitySettings = new FPS.PlayerMovementData
        {
            NominalVelocity = 8.0f
        };

        [Header("Sprint Movement")]

        [SerializeField]
        private bool _enableSprint = true;

        [SerializeField]
        private FPS.PlayerMovementData _sprintVelocitySettings = new FPS.PlayerMovementData
        {
            NominalVelocity = 16.0f
        };

        [Header("Aim Movement")]

        [SerializeField]
        private bool _enableAim = true;

        [SerializeField]
        private FPS.PlayerMovementData _aimVelocitySettings = new FPS.PlayerMovementData
        {
            NominalVelocity = 2.5f
        };

        [Header("Jump")]

        [SerializeField]
        private bool _enableJump = true;

        [Min(0.1f)]
        [SerializeField]
        private float _jumpHeight = 4.0f;

        [Header("Physics")]

        [Min(0.1f)]
        [SerializeField]
        private float _characterMass = 90.0f;

        [Min(0.01f)]
        [SerializeField]
        private float _accelerationTime = 0.3f;

        [Min(0.01f)]
        [SerializeField]
        private float _decelerationTime = 0.2f;

        [Header("Gravity")]

        [SerializeField]
        private bool _enableGravity = true;

        [Min(0.1f)]
        [SerializeField]
        private float _gravityAcceleration = 9.81f;

        [Min(0.1f)]
        [SerializeField]
        private float _gravityRiseMultiplier = 1.0f;

        [Header("Ground Detection")]

        [SerializeField]
        private Vector3 _groundCheckOffset = Vector3.zero;

        [Min(0.0f)]
        [SerializeField]
        private float _groundCheckRadius = 0.25f;

        [SerializeField]
        private LayerMask _groundLayers = -1;

        #endregion

        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private Rigidbody _rigidbodyComponent;

        private FPS.PlayerInputController _playerInputControllerComponent;

        private FPS.EPlayerMovementState _movementState = FPS.EPlayerMovementState.Airborne;

        private Vector3 _currentForwardDirection = Vector3.forward;

        private Vector3 _currentRightDirection = Vector3.right;

        private bool _isGrounded = false;

        private bool _isSprinting = false;

        private bool _isAiming = false;

        #endregion

        #region PUBLIC MEMBERS

        // ============================================================================================
        //  PUBLIC MEMBERS
        // ============================================================================================

        public FPS.EPlayerMovementState MovementState => _movementState;

        public Vector3 CurrentVelocity => _rigidbodyComponent.velocity;

        public bool IsGrounded => _isGrounded;

        public bool IsSprinting => _isSprinting;

        public bool IsAiming => _isAiming;

        public event Action OnJump;

        public event Action OnFall;

        public event Action OnLanding;

        #endregion

        #region PRIVATE METHODS

        // ============================================================================================
        //  PRIVATE METHODS
        // ============================================================================================

        private void OnJumpPressed()
        {
            if (_enableJump == false || _isGrounded == false)
            {
                return;
            }

            float jumpVelocity = Mathf.Sqrt(
                2.0f *
                _gravityAcceleration *
                _gravityRiseMultiplier *
                _jumpHeight
            );

            Vector3 velocity = _rigidbodyComponent.velocity;

            velocity.y = jumpVelocity;

            _rigidbodyComponent.velocity = velocity;

            OnJump?.Invoke();
        }

        private void UpdateHeldStates()
        {
            _isSprinting =
                _enableSprint &&
                _playerInputControllerComponent.SprintHeld;

            _isAiming =
                _enableAim &&
                _playerInputControllerComponent.AimHeld;
        }

        private void SetupRigidbody()
        {
            _rigidbodyComponent.useGravity = false;

            _rigidbodyComponent.mass = _characterMass;

            _rigidbodyComponent.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void CheckGround()
        {
            bool wasGrounded = _isGrounded;

            Vector3 groundCheckOrigin =
                transform.position +
                transform.TransformDirection(_groundCheckOffset);

            _isGrounded = Physics.CheckSphere(
                groundCheckOrigin,
                _groundCheckRadius,
                _groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (wasGrounded && !_isGrounded)
            {
                OnFall?.Invoke();
            }

            if (!wasGrounded && _isGrounded)
            {
                OnLanding?.Invoke();
            }
        }

        private float GetDirectionalTargetSpeed(
            Vector2 lookVector,
            FPS.PlayerMovementData movementData)
        {
            if (lookVector.sqrMagnitude < 0.001f)
            {
                return 0.0f;
            }

            float forwardWeight = Mathf.Max(0.0f, lookVector.y);
            float backWeight = Mathf.Max(0.0f, -lookVector.y);
            float rightWeight = Mathf.Max(0.0f, lookVector.x);
            float leftWeight = Mathf.Max(0.0f, -lookVector.x);

            float totalWeight =
                forwardWeight +
                backWeight +
                rightWeight +
                leftWeight;

            if (totalWeight < 0.001f)
            {
                return movementData.NominalVelocity;
            }

            float blendedMultiplier =
                (
                    movementData.ForwardMultiplier * forwardWeight +
                    movementData.BackMultiplier * backWeight +
                    movementData.RightMultiplier * rightWeight +
                    movementData.LeftMultiplier * leftWeight
                ) / totalWeight;

            return movementData.NominalVelocity * blendedMultiplier;
        }

        private FPS.PlayerMovementData ResolveMovementMode()
        {
            if (_enableAim && _isAiming)
            {
                return _aimVelocitySettings;
            }

            if (_enableSprint && _isSprinting)
            {
                return _sprintVelocitySettings;
            }

            return _walkVelocitySettings;
        }

        private void ApplyMovementDirection()
        {
            _currentForwardDirection =
                _movementDirectionRootReference.forward.normalized;

            _currentRightDirection =
                _movementDirectionRootReference.right.normalized;
        }

        private void ApplyTranslationMovement()
        {
            Vector2 moveVector =
                _playerInputControllerComponent.MoveVector;

            bool wantsToMove =
                moveVector.sqrMagnitude > 0.001f;

            FPS.PlayerMovementData currentMovementData =
                ResolveMovementMode();

            float targetSpeed =
                GetDirectionalTargetSpeed(
                    moveVector,
                    currentMovementData
                );

            if (!wantsToMove)
            {
                targetSpeed = 0.0f;
            }

            if (!_enableWalk && !_isSprinting && !_isAiming)
            {
                targetSpeed = 0.0f;
            }

            Vector3 targetHorizontalVelocity = Vector3.zero;

            if (wantsToMove && targetSpeed > 0.0f)
            {
                Vector3 inputDirection =
                    (
                        _currentForwardDirection * moveVector.y +
                        _currentRightDirection * moveVector.x
                    ).normalized;

                targetHorizontalVelocity =
                    inputDirection * targetSpeed;
            }

            Vector3 currentHorizontalVelocity =
                new Vector3(
                    _rigidbodyComponent.velocity.x,
                    0.0f,
                    _rigidbodyComponent.velocity.z
                );

            Vector3 velocityDelta =
                targetHorizontalVelocity -
                currentHorizontalVelocity;

            float accelerationTime =
                wantsToMove
                    ? _accelerationTime
                    : _decelerationTime;

            Vector3 requiredAcceleration =
                velocityDelta / accelerationTime;

            _rigidbodyComponent.AddForce(
                requiredAcceleration,
                ForceMode.Acceleration
            );
        }

        private void ApplyGravity()
        {
            if (!_enableGravity)
            {
                return;
            }

            float gravityMultiplier = _rigidbodyComponent.velocity.y > 0.0f ? _gravityRiseMultiplier : 1.0f;

            Vector3 gravityAcceleration =
                Vector3.down *
                _gravityAcceleration *
                gravityMultiplier;

            _rigidbodyComponent.AddForce(gravityAcceleration, ForceMode.Acceleration);
        }

        private void UpdateMovementState()
        {
            Vector3 horizontalVelocity =
                new Vector3(
                    _rigidbodyComponent.velocity.x,
                    0.0f,
                    _rigidbodyComponent.velocity.z
                );

            if (!_isGrounded)
            {
                _movementState = FPS.EPlayerMovementState.Airborne;
            }
            else if (horizontalVelocity.sqrMagnitude < 0.01f)
            {
                _movementState = FPS.EPlayerMovementState.Idle;
            }
            else if (_isSprinting)
            {
                _movementState = FPS.EPlayerMovementState.Sprinting;
            }
            else
            {
                _movementState = FPS.EPlayerMovementState.Walking;
            }
        }

        #endregion

        #region UNITY MONOBEHAVIOUR METHODS

        // ============================================================================================
        //  UNITY MONOBEHAVIOUR METHODS
        // ============================================================================================

        private void Awake()
        {
            _rigidbodyComponent = GetComponent<Rigidbody>();

            _playerInputControllerComponent = GetComponent<FPS.PlayerInputController>();

            _playerInputControllerComponent.JumpPressed += OnJumpPressed;

            _movementDirectionRootReference = _movementDirectionRootReference == null ? transform : _movementDirectionRootReference;

            SetupRigidbody();
        }

        private void OnDestroy()
        {
            if (_playerInputControllerComponent != null)
            {
                _playerInputControllerComponent.JumpPressed -= OnJumpPressed;
            }
        }

        private void FixedUpdate()
        {
            CheckGround();

            UpdateHeldStates();

            ApplyGravity();

            ApplyMovementDirection();

            ApplyTranslationMovement();

            UpdateMovementState();
        }

        #endregion
    }
}

