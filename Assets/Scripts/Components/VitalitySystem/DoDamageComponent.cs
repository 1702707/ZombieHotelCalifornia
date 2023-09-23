using Unity.VisualScripting;
using UnityEngine;

namespace Controller.Components.VitalitySystem
{
    public class DoDamageComponent: MonoBehaviour
    {
        [SerializeField] private int _damage;

        public int Damage => _damage;
        
        private void OnCollisionEnter(Collision collision)
        {
            HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null)
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