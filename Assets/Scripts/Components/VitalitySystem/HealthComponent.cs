using System;
using Controller.Components.Events;
using UnityEngine;

namespace Controller.Components.VitalitySystem
{
    public abstract class HealthComponent: MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private bool _iKickable;
        [SerializeField] private EntityType _ownerType;
        [SerializeField] protected ComboEvent _comboEvent;
        [SerializeField] private float _height;
        public float toppleForce;

        public int CurrentHP => _currentHp;
        public int MaxHP => _maxHealth;
        public EntityType OwnerType => _ownerType;
        public float Height => _height;

        private int _currentHp;
        private StaggeredComponent _staggered;

        private void Awake()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            _currentHp = _maxHealth;
        }

        protected abstract void OnDamage();
        protected abstract void OnKick(Vector3 force, Action callback);

        public virtual void DoDamage(DamageData contact, int damage)
        {
            if (_currentHp > 0)
            {
                _currentHp = Mathf.Clamp(_currentHp - damage, 0, _maxHealth);

                var damageEvent = contact;
                damageEvent.Type = ComboType.Damage;
                _comboEvent?.TriggerEvent(damageEvent);
                
                if (contact.Target.tag == "Head")
                {
                    var headshot = contact;
                    headshot.Type = ComboType.Headshot;
                    _comboEvent?.TriggerEvent(headshot);
                    _currentHp = 0;
                    OnHeadshot();
                }
                if (_currentHp <= 0)
                {
                    var kill = contact;
                    kill.Type = _ownerType == EntityType.Player? ComboType.Lose: ComboType.Kill;
                    _comboEvent?.TriggerEvent(kill);
                    OnDeath();
                }
                
                OnDamage();
            }
        }

        protected abstract void OnDeath();

        protected abstract void OnHeadshot();

        public virtual void DoKick(DamageData data, Action callback)
        {
            //Kick Audio
            if (_iKickable)
            {
                _comboEvent.TriggerEvent(data);
                OnKick(data.Impulse, callback);
            }
        }

        public virtual void DoPunch()
        {
            //Punch audio
            if(IsDead)
                return;
            Staggered.Do();
            _comboEvent.TriggerEvent(new DamageData
            {
                HitPoint = transform.position,
                Height = _height,
                Target = gameObject,
                Impulse = default,
                SourceID = 0,
                Type = ComboType.Stagger,
            });
            onPunch();
        }

        public StaggeredComponent Staggered
        {
            get
            {
                if (_staggered == null)
                {
                    _staggered = GetComponent<StaggeredComponent>() ?? gameObject.AddComponent<StaggeredComponent>();
                }

                return _staggered;
            }
        }

        public bool IsDead => _currentHp <= 0;

        protected abstract void onPunch();
    }
}

public struct DamageData
{
    public ComboType Type;
    public Vector3 HitPoint;
    public float Height;
    public GameObject Target;
    public Vector3 Impulse;
    public int SourceID;
}