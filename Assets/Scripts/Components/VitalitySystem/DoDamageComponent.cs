using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Controller.Components.VitalitySystem
{
    public class DoDamageComponent: MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private EntityType _target;

        public int Damage => _damage;
        public EntityType Target => _target;


        private void OnCollisionEnter(Collision collision)
        {
            //Debug.Log($" Enter {collision.gameObject.name}");
            HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
            var contact = collision.contacts.First();
            var data = new DamageData
            {
                HitPoint = contact.point,
                Collider = contact.otherCollider.gameObject,
                Impulse = contact.impulse,
                ID = Convert.ToInt32(collision.gameObject.name)
            };
            if (health != null && health.OwnerType == _target)
            {
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