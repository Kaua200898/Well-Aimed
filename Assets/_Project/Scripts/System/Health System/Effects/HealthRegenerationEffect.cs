using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(FPS.HealthSystem))]
    public class HealthRegenerationEffect : MonoBehaviour
    {
        #region UNITY INSPECTOR FIELDS

        // ============================================================================================
        //  UNITY INSPECTOR FIELDS
        // ============================================================================================

        [Header("Effect Activation")]

        public bool IsEnabled = true;

        [Header("Effect Parameters")]

        [Min(0.0f)]
        [SerializeField]
        private float _healthRegenerationStep = 5.0f;

        [Min(1.0f)]
        [SerializeField]
        private float _healthRegenerationInterval = 5.0f;

        [Min(0.0f)]
        [SerializeField]
        private float _healthRegenerationDamageCooldown = 10.0f;

        #endregion

        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private FPS.HealthSystem _healthSystemComponent;

        private float _regenerationIntervalTimer = 0.0f;

        private float _lastDamageTime = Mathf.NegativeInfinity;

        #endregion

        #region PRIVATE METHODS

        // ============================================================================================
        //  PRIVATE METHODS
        // ============================================================================================

        private void OnDamage(FPS.DamageData damageData)
        {
            _regenerationIntervalTimer = 0.0f;

            _lastDamageTime = Time.time;
        }

        private void ApplyEffect()
        {
            if (IsEnabled == true && _healthSystemComponent != null)
            {
                float currentHealth = _healthSystemComponent.CurrentHealth;

                float baseHealth = _healthSystemComponent.BaseHealth;

                float healthDelta = baseHealth - currentHealth;

                if (healthDelta > 0.0f && Time.time >= _lastDamageTime + _healthRegenerationDamageCooldown)
                {
                    _regenerationIntervalTimer += Time.deltaTime;

                    if (_regenerationIntervalTimer >= _healthRegenerationInterval)
                    {
                        _regenerationIntervalTimer -= _healthRegenerationInterval;

                        float healAmount = (healthDelta > _healthRegenerationStep) ? _healthRegenerationStep : healthDelta;

                        _healthSystemComponent.Heal(healAmount);
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

        private void Update()
        {
            ApplyEffect();
        }

        #endregion
    }
}
