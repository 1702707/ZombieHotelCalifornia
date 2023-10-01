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
        [SerializeField] private ParticleSystem _headshotEffect;
        [SerializeField] private float _height;

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
            if (_ownerType == EntityType.Enemy)
            {
                if (contact.Collider.tag == "Head")
                {
                   if(_headshotEffect != null) 
                       _headshotEffect?.Play();
                    _headshotEvent.TriggerEvent(contact.HitPoint); 
                }
            }
            
            if (_currentHp > 0)
            {
                _currentHp = Mathf.Clamp(_currentHp - damage, 0, _maxHealth);
                OnDamage();
                
                if(_currentHp == 0)
                    _killEnemyEvent.TriggerEvent(contact);
            }
        }

        public virtual void DoKick(Vector3 force, Action callback)
        {
            if (_iKickable)
            {
                _status = HealthState.Knocked;
                if (Staggered.InProgress)
                {
                    
                }
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
    public GameObject Collider;
    public Vector3 Impulse;
    public int ID;
}