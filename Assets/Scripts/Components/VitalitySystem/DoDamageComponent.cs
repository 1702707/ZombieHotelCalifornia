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
            Debug.Log(" Enter");
            HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null && health.OwnerType == _target)
            {
                health.DoDamage(collision.impulse.magnitude, _damage);
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
    }
}