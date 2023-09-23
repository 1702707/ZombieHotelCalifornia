using UnityEngine;

namespace Controller.Components.VitalitySystem
{
    public abstract class HealthComponent: MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private bool _iKickable;

        public int CurrentHP => _currentHp;
        public int MaxHP => _maxHealth;

        private int _currentHp;

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
                OnKick(force);
            }
        }
    }
}