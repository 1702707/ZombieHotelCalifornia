using System;
using System.Linq;
using UnityEngine;

namespace Controller.Components.VitalitySystem
{
    [RequireComponent(typeof(SphereCollider))]
    public class DoDamageComponent: MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private EntityType _target;
        [SerializeField] private Animator _attackAnimator;
        private HealthComponent _health;

        public int Damage => _damage;
        public EntityType Target => _target;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();
        }

        private void OnTriggerStay(Collider other)
        {
            if(_attackAnimator == null)
                return;
            
            HealthComponent health = other.gameObject.GetComponent<HealthComponent>();
            if (health != null && health.OwnerType == _target)
            {
                _attackAnimator.SetTrigger("Attack");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(_attackAnimator == null)
                return;
            
            HealthComponent health = other.gameObject.GetComponent<HealthComponent>();
            if (health != null && health.OwnerType == _target)
            {
                _attackAnimator.ResetTrigger("Attack");
            }
        }


        private void OnCollisionEnter(Collision collision)
        {
            if(_health != null && (_health.Staggered || _health.IsDead))
                return;
            //Debug.Log($" Enter {collision.gameObject.name}");
            HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null && health.OwnerType == _target)
            {
                var contact = collision.contacts.First();
                int.TryParse(gameObject.name.Split('_').Last(), out var id);
                var data = new DamageData
                {
                    HitPoint = contact.point,
                    Target = contact.otherCollider.gameObject,
                    Impulse = contact.impulse,
                    SourceID = id,
                    Height = health.Height,
                    Type = ComboType.Damage	
                };
                health.DoDamage(data, _damage);
            }
        }
        
        //private void OnCollisionEnter(Collision collision)
        // {
        //     Zombie hitZombie = collision.gameObject.GetComponent<Zombie>();
        //     if (hitZombie != null)
        //     {
        //         Debug.Log(collision.impulse.magnitude);
        //         if (collision.impulse.magnitude > hitZombie.toppleForce)
        //             if (!hitZombie.isDead)
        //                 StartCoroutine(hitZombie.ZombieDie());
        //     }
        // }
        public void SetTarget(EntityType enemy)
        {
            _target = enemy;
        }

        public void SetDamage(int damage)
        {
            _damage = damage;
        }
    }
}