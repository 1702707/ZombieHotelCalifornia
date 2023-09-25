using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller.Components.VitalitySystem
{
    public abstract class HealthComponent: MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private bool _iKickable;
        [SerializeField] private EntityType _ownerType;

        public int CurrentHP => _currentHp;
        public int MaxHP => _maxHealth;
        public EntityType OwnerType => _ownerType;
        public HealthState State => _status;

        private int _currentHp;
        private HealthState _status;

        private void Awake()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            _currentHp = _maxHealth;
        }

        protected abstract void OnDamage();
        protected abstract void OnKick(Vector3 force);

        public virtual void DoDamage(float impulse, int damage)
        {
            DoDamage(damage);
        }

        public virtual void DoDamage(int damage)
        {
            if (_currentHp > 0)
            {
                _currentHp = Mathf.Clamp(_currentHp - damage, 0, _maxHealth);
                OnDamage();
            }
        }

        public virtual void DoKick(Vector3 force)
        {
            if (_iKickable)
            {
                _status = HealthState.Knocked;
                OnKick(force);
            }
        }

        public virtual void DoPunch()
        {
            onPunch();
        }

        protected abstract void onPunch();
    }
}

public enum HealthState
{
    Knocked,
    Staggered,
    Normal
}