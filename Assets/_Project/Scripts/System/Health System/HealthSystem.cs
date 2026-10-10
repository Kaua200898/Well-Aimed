using System;
using System.Linq.Expressions;
using UnityEngine;

namespace FPS
{ 
    public class HealthSystem : MonoBehaviour
    {
        #region UNITY INSPECTOR FIELDS

        // ============================================================================================
        //  UNITY INSPECTOR FIELDS
        // ============================================================================================

        [Header("Health Parameters")]

        [SerializeField]
        private bool _isInvincible = false;

        [Min(0.1f)]
        [SerializeField] 
        private float _maximumHealth = 100.0f;

        [Min(0.1f)]
        [SerializeField]
        private float _baseHealth = 100.0f;

        #endregion

        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private float _currentHealth = 0.0f;

        private bool _isAlive = true;

        #endregion

        #region PUBLIC MEMBERS

        // ============================================================================================
        //  PUBLIC MEMBERS
        // ============================================================================================

        public float MaximumHealth => _maximumHealth;

        public float BaseHealth => _baseHealth;

        public float CurrentHealth => _currentHealth;

        public bool IsInvincible => _isInvincible;

        public bool IsAlive => _isAlive;

        public event Action<float, float> OnHealthChange;

        public event Action<FPS.DamageData> OnDamage;

        public event Action OnInvincibilityStart;

        public event Action OnInvincibilityStop;

        public event Action OnRevival;

        public event Action OnDeath;

        #endregion

        #region PUBLIC METHODS

        // ============================================================================================
        //  PUBLIC METHODS
        // ============================================================================================

        public void Heal(float healAmount)
        {
            if (_isAlive == true && healAmount > 0.0f)
            {
                float oldHealth = _currentHealth;

                _currentHealth = Mathf.Clamp(oldHealth + healAmount, 0.0f, _maximumHealth);

                OnHealthChange?.Invoke(_currentHealth, oldHealth);
            }
        }

        public void SetHealth(float healthValue)
        {
            if (_isAlive == true && healthValue >= 0.0f)
            {
                float oldHealth = _currentHealth;

                _currentHealth = Mathf.Clamp(healthValue, 0.0f, _maximumHealth);

                OnHealthChange?.Invoke(_currentHealth, oldHealth);

                if (_currentHealth <= 0.0f)
                {
                    Die();
                }
            }
        }

        public void ResetHealth()
        {
            if (_isAlive == true)
            {
                float oldHealth = _currentHealth;

                _currentHealth = _baseHealth;

                OnHealthChange?.Invoke(_currentHealth, oldHealth);
            }
        }

        public void Damage(FPS.DamageData damageData)
        {
            if (_isAlive == true && _isInvincible == false && damageData.Value > 0.0f)
            {
                float oldHealth = _currentHealth;

                _currentHealth = Mathf.Clamp(_currentHealth - damageData.Value, 0.0f, _maximumHealth);

                OnHealthChange?.Invoke(_currentHealth, oldHealth);

                OnDamage?.Invoke(damageData);

                if (_currentHealth <= 0.0f)
                {
                    Die();
                }
            }
        }

        public void SetInvincibility(bool isInvincible)
        {
            bool oldInvincibility = _isInvincible;

            if (isInvincible == true && oldInvincibility == false)
            {
                OnInvincibilityStart?.Invoke();
            }
            else if (isInvincible == false && oldInvincibility == true)
            {
                OnInvincibilityStop?.Invoke();
            }

            _isInvincible = isInvincible;
        }

        public void Revive()
        {
            if (_isAlive == false)
            { 
                _isAlive = true;

                ResetHealth();

                OnRevival?.Invoke();
            }
        }

        public void Die()
        {
            if (_isAlive == true)
            { 
                _isAlive = false;

                float oldHealth = _currentHealth;

                if (oldHealth > 0.0f)
                {
                    _currentHealth = 0.0f;

                    OnHealthChange?.Invoke(_currentHealth, oldHealth);
                }

                OnDeath?.Invoke();
            }
        }

        #endregion

        #region UNITY MONOBEHAVIOUR METHODS

        // ============================================================================================
        //  UNITY MONOBEHAVIOUR METHODS
        // ============================================================================================

        private void Awake()
        {
            _currentHealth = _baseHealth;
        }

        #endregion
    }
}
