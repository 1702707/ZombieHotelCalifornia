using System;
using Controller.Components.Events;
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
        [SerializeField] private HeadshotEvent _headshotEvent;
        [SerializeField] private KillEnemyEvent _killEnemyEvent;
        [SerializeField] private KnockEvent _knockEvent;
        [SerializeField] private DamageEvent _damageEvent;
        [SerializeField] private ParticleSystem _headshotEffect;
        [SerializeField] private float _height;
        public float toppleForce;

        public int CurrentHP => _currentHp;
        public int MaxHP => _maxHealth;
        public EntityType OwnerType => _ownerType;
        public HealthState State => _status;
        public float Height => _height;

        private int _currentHp;
        private HealthState _status;
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

                if (_ownerType == EntityType.Enemy)
                {
                    _damageEvent.TriggerEvent(contact);
                    contact.HitPoint.y = _height;
                    
                    if (contact.Target.tag == "Head")
                    {
                        if(_headshotEffect != null) 
                            _headshotEffect?.Play();
                        
                        _headshotEvent.TriggerEvent(contact);
                        _currentHp = 0;
                        OnHeadshot();
                    }
                    else if (_currentHp <= 0)
                    {
                        _killEnemyEvent.TriggerEvent(contact);
                        OnDeath();
                    }
                }
                OnDamage();
            }
        }

        protected abstract void OnDeath();

        protected abstract void OnHeadshot();

        public virtual void DoKick(Vector3 force, Action callback)
        {
            if (_iKickable)
            {
                _status = HealthState.Knocked;
                
                OnKick(force, callback);
            }
        }

        public virtual void DoPunch()
        {
            Staggered.Do();
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

        protected abstract void onPunch();
        
        
        // private void OnCollisionEnter(Collision collision)
        // {
        //     if (Staggered.InProgress)
        //     {
        //         var health = collision.gameObject.GetComponent<HealthComponent>();
        //         if (health != null && health.OwnerType == EntityType.Enemy)
        //         {
        //             health.Staggered.Do();
        //         }
        //     }
        // }
    }
}

public enum HealthState
{
    Knocked,
    Staggered,
    Normal
}

public class DamageData
{
    public Vector3 HitPoint;
    public float Height;
    public GameObject Target;
    public Vector3 Impulse;
    public int SourceID;
}