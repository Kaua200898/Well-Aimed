using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    [RequireComponent(typeof(Collider))]
    public class HazardController : MonoBehaviour
    {
        #region UNITY INSPECTOR FIELDS

        // ============================================================================================
        //  UNITY INSPECTOR FIELDS
        // ============================================================================================

        [Header("Hazard Parameters")]

        public bool EnableHazard = true;

        [Header("Damage Parameters")]

        public bool IsInstantKill = false;

        [Min(0.1f)]
        public float DamageValue = 1.0f;

        public bool IsDamageContinuous = false;

        [Min(0.1f)]
        public float DamageTickFrequency = 1.0f;

        [Header("Effects Parameters")]

        [Min(0.0f)]
        public float HazardKnockbackForce = 1.0f;

        [Header("Target Detection")]

        public string TargetTag = string.Empty;

        public LayerMask TargetLayerMask = -1;

        #endregion

        #region PRIVATE MEMBERS

        // ============================================================================================
        //  PRIVATE MEMBERS
        // ============================================================================================

        private Collider _colliderComponent;

        private readonly Dictionary<FPS.HealthSystem, float> _damageTimers = new();

        #endregion

        #region PRIVATE METHODS

        // ============================================================================================
        //  PRIVATE METHODS
        // ============================================================================================

        private bool IsValidTarget(GameObject target)
        {
            bool isLayerValid = ((TargetLayerMask.value & (1 << target.layer)) != 0);

            bool isTagValid = (string.IsNullOrWhiteSpace(TargetTag) || target.CompareTag(TargetTag));

            return isLayerValid && isTagValid;
        }

        private void RegisterContinuousTarget(GameObject target)
        {
            FPS.HealthSystem healthSystem = target.GetComponent<FPS.HealthSystem>();

            if (healthSystem != null && _damageTimers.ContainsKey(healthSystem) == false)
            {
                _damageTimers.Add(healthSystem, 0.0f);
            }
        }

        private void UnregisterContinuousTarget(GameObject target)
        {
            FPS.HealthSystem healthSystem = target.GetComponent<FPS.HealthSystem>();

            if (healthSystem != null)
            {
                _damageTimers.Remove(healthSystem);
            }
        }

        private void ApplyDamage(GameObject target, Vector3 contactPoint)
        {
            if (EnableHazard == true)
            {
                FPS.HealthSystem healthSystem = target.GetComponent<FPS.HealthSystem>();

                if (healthSystem != null)
                {
                    Vector3 direction = target.transform.position - transform.position;

                    if (direction.sqrMagnitude > Mathf.Epsilon)
                    {
                        direction.Normalize();
                    }
                    else
                    {
                        direction = Vector3.up;
                    }

                    if (IsInstantKill == true)
                    {
                        healthSystem.Die();
                    }
                    else
                    {
                        FPS.DamageData damageData = new FPS.DamageData
                        {
                            Value = DamageValue,
                            Knockback = HazardKnockbackForce,
                            Direction = direction,
                            Point = contactPoint,
                            Source = gameObject
                        };

                        healthSystem.Damage(damageData);
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
            _colliderComponent = GetComponent<Collider>();
        }

        private void FixedUpdate()
        {
            if (EnableHazard == true && IsDamageContinuous == true)
            {
                List<FPS.HealthSystem> targetsToRemove = new List<FPS.HealthSystem>();

                foreach (var pair in _damageTimers)
                {
                    FPS.HealthSystem healthSystem = pair.Key;

                    if (healthSystem == null)
                    {
                        targetsToRemove.Add(healthSystem);
                    }
                    else
                    {
                        _damageTimers[healthSystem] += Time.deltaTime;

                        if (_damageTimers[healthSystem] >= DamageTickFrequency)
                        {
                            _damageTimers[healthSystem] = 0.0f;

                            ApplyDamage(healthSystem.gameObject, healthSystem.transform.position);
                        }
                    }
                }

                if (targetsToRemove.Count > 0)
                {
                    foreach (FPS.HealthSystem target in targetsToRemove)
                    {
                        _damageTimers.Remove(target);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsValidTarget(other.gameObject) == true)
            {
                if (IsDamageContinuous == true)
                {
                    RegisterContinuousTarget(other.gameObject);
                }
                else
                {
                    ApplyDamage(other.gameObject, other.ClosestPoint(transform.position));
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsDamageContinuous == true)
            {
                UnregisterContinuousTarget(other.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (IsValidTarget(collision.gameObject) == true)
            {
                Vector3 point = (collision.contactCount > 0) ? collision.GetContact(0).point : collision.transform.position;

                if (IsDamageContinuous == true)
                {
                    RegisterContinuousTarget(collision.gameObject);
                }
                else
                {
                    ApplyDamage(collision.gameObject, point);
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (IsDamageContinuous == true)
            {
                UnregisterContinuousTarget(collision.gameObject);
            }
        }

        #endregion
    }
}
