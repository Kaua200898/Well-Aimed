using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(FPS.HealthSystem))]
    public class DamageKnockbackEffect : MonoBehaviour
    {
        #region UNITY INSPECTOR FIELDS

        // ============================================================================================
        //  UNITY INSPECTOR FIELDS
        // ============================================================================================

        [Header("Effect Activation")]

        public bool IsEnabled = true;

        [Header("Effect Parameters")]

        [Range(0.0f, 1.0f)]
        public float KnockbackResistance = 0.0f;

        #endregion

        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private Rigidbody _rigidbodyComponent;

        private FPS.HealthSystem _healthSystemComponent;

        #endregion

        #region PRIVATE METHODS

        // ============================================================================================
        //  PRIVATE METHODS
        // ============================================================================================

        private void OnDamage(FPS.DamageData damageData)
        {
            if (IsEnabled == true && _rigidbodyComponent != null && _healthSystemComponent != null)
            {
                Vector3 knockbackDirection = damageData.Direction;

                if (knockbackDirection.sqrMagnitude >= Mathf.Epsilon)
                {
                    knockbackDirection.Normalize();

                    float knockbackMagnitude = Mathf.Max(0.0f, damageData.Knockback);

                    float calculatedKnockback = knockbackMagnitude * (1.0f - KnockbackResistance);

                    Vector3 currentVelocity = _rigidbodyComponent.velocity;

                    if (calculatedKnockback >= 0.0f)
                    {
                        if (Vector3.Dot(currentVelocity, knockbackDirection) < 0.0f)
                        {
                            currentVelocity = Vector3.ProjectOnPlane(currentVelocity, knockbackDirection);

                            _rigidbodyComponent.velocity = currentVelocity;
                        }

                        _rigidbodyComponent.AddForce(knockbackDirection * calculatedKnockback, ForceMode.VelocityChange);
                    }
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
            _rigidbodyComponent = GetComponent<Rigidbody>();

            _healthSystemComponent = GetComponent<FPS.HealthSystem>();
        }

        private void OnEnable()
        {
            if (_healthSystemComponent != null)
            {
                _healthSystemComponent.OnDamage += OnDamage;
            }
        }

        private void OnDisable()
        {
            if (_healthSystemComponent != null)
            {
                _healthSystemComponent.OnDamage -= OnDamage;
            }
        }

        #endregion
    }
}
