using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace FPS
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private FPSPlayerInputActions _playerInputActions;

        #endregion

        #region PUBLIC MEMBERS

        // ============================================================================================
        //  PUBLIC MEMBERS
        // ============================================================================================

        public Vector2 MoveVector { get; private set; } = Vector2.zero;

        public Vector2 LookVector { get; private set; } = Vector2.zero;

        public bool SprintHeld { get; private set; } = false;

        public bool JumpHeld { get; private set; } = false;

        public bool FireHeld { get; private set; } = false;

        public bool AimHeld { get; private set; } = false;

        public bool FlashlightHeld { get; private set; } = false;

        public event Action SprintPressed;

        public event Action JumpPressed;

        public event Action FirePressed;

        public event Action AimPressed;

        public event Action ReloadPressed;

        public event Action FlashlightPressed;

        public event Action PausePressed;

        #endregion

        #region PRIVATE METHODS

        // ============================================================================================
        //  PRIVATE METHODS
        // ============================================================================================

        private void OnMove(InputAction.CallbackContext context)
        {
            MoveVector = context.ReadValue<Vector2>();
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            LookVector = context.ReadValue<Vector2>();
        }

        private void OnFire(InputAction.CallbackContext context)
        {
            if (context.started == true || context.performed == true)
            {
                if (FireHeld == false)
                {
                    FireHeld = true;

                    FirePressed?.Invoke();
                }
            }
            else
            {
                FireHeld = false;
            }
        }

        private void OnAim(InputAction.CallbackContext context)
        {
            if (context.started == true || context.performed == true)
            {
                if (AimHeld == false)
                {
                    AimHeld = true;

                    AimPressed?.Invoke();
                }
            }
            else
            {
                AimHeld = false;
            }
        }
        private void OnSprint(InputAction.CallbackContext context)
        {
            if (context.started == true || context.performed == true)
            {
                if (SprintHeld == false)
                {
                    SprintHeld = true;

                    SprintPressed?.Invoke();
                }
            }
            else
            {
                SprintHeld = false;
            }
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.started == true || context.performed == true)
            {
                if (JumpHeld == false)
                {
                    JumpHeld = true;

                    JumpPressed?.Invoke();
                }
            }
            else
            {
                JumpHeld = false;
            }
        }

        private void OnReload(InputAction.CallbackContext context)
        {
            ReloadPressed?.Invoke();
        }

        private void OnFlashlight(InputAction.CallbackContext context)
        {
            if (context.started == true || context.performed == true)
            {
                if (FlashlightHeld == false)
                {
                    FlashlightHeld = true;

                    FlashlightPressed?.Invoke();
                }
            }
            else
            {
                FlashlightHeld = false;
            }
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            PausePressed?.Invoke();
        }

        #endregion

        #region UNITY MONOBEHAVIOUR METHODS

        // ============================================================================================
        //  UNITY MONOBEHAVIOUR METHODS
        // ============================================================================================

        private void Awake()
        {
            _playerInputActions = new FPSPlayerInputActions();
        }

        private void OnEnable()
        {
            _playerInputActions.Enable();

            _playerInputActions.Gameplay.Move.performed += OnMove;
            _playerInputActions.Gameplay.Move.canceled  += OnMove;

            _playerInputActions.Gameplay.Look.performed += OnLook;
            _playerInputActions.Gameplay.Look.canceled  += OnLook;

            _playerInputActions.Gameplay.Sprint.started   += OnSprint;
            _playerInputActions.Gameplay.Sprint.performed += OnSprint;
            _playerInputActions.Gameplay.Sprint.canceled  += OnSprint;

            _playerInputActions.Gameplay.Jump.started   += OnJump;
            _playerInputActions.Gameplay.Jump.performed += OnJump;
            _playerInputActions.Gameplay.Jump.canceled  += OnJump;

            _playerInputActions.Gameplay.Fire.started   += OnFire;
            _playerInputActions.Gameplay.Fire.performed += OnFire;
            _playerInputActions.Gameplay.Fire.canceled  += OnFire;

            _playerInputActions.Gameplay.Aim.started   += OnAim;
            _playerInputActions.Gameplay.Aim.performed += OnAim;
            _playerInputActions.Gameplay.Aim.canceled  += OnAim;

            _playerInputActions.Gameplay.Reload.performed += OnReload;

            _playerInputActions.Gameplay.Flashlight.started   += OnFlashlight;
            _playerInputActions.Gameplay.Flashlight.performed += OnFlashlight;
            _playerInputActions.Gameplay.Flashlight.canceled  += OnFlashlight;

            _playerInputActions.Gameplay.Pause.performed += OnPause;

            _playerInputActions.UI.Pause.performed += OnPause;
        }

        private void OnDisable()
        {
            _playerInputActions.Gameplay.Move.performed -= OnMove;
            _playerInputActions.Gameplay.Move.canceled  -= OnMove;

            _playerInputActions.Gameplay.Look.performed -= OnLook;
            _playerInputActions.Gameplay.Look.canceled  -= OnLook;

            _playerInputActions.Gameplay.Sprint.started   -= OnSprint;
            _playerInputActions.Gameplay.Sprint.performed -= OnSprint;
            _playerInputActions.Gameplay.Sprint.canceled  -= OnSprint;

            _playerInputActions.Gameplay.Jump.started   -= OnJump;
            _playerInputActions.Gameplay.Jump.performed -= OnJump;
            _playerInputActions.Gameplay.Jump.canceled  -= OnJump;

            _playerInputActions.Gameplay.Fire.started   -= OnFire;
            _playerInputActions.Gameplay.Fire.performed -= OnFire;
            _playerInputActions.Gameplay.Fire.canceled  -= OnFire;

            _playerInputActions.Gameplay.Aim.started   -= OnAim;
            _playerInputActions.Gameplay.Aim.performed -= OnAim;
            _playerInputActions.Gameplay.Aim.canceled  -= OnAim;

            _playerInputActions.Gameplay.Reload.performed -= OnReload;

            _playerInputActions.Gameplay.Flashlight.started   -= OnFlashlight;
            _playerInputActions.Gameplay.Flashlight.performed -= OnFlashlight;
            _playerInputActions.Gameplay.Flashlight.canceled  -= OnFlashlight;

            _playerInputActions.Gameplay.Pause.performed -= OnPause;

            _playerInputActions.UI.Pause.performed -= OnPause;

            _playerInputActions.Disable();
        }

        #endregion
    }
}


