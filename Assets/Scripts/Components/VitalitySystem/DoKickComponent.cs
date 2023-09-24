using UnityEngine;

namespace Controller.Components.VitalitySystem
{
    public class DoKickComponent: MonoBehaviour
    {
        [SerializeField] private EntityType _target;
        
        private void OnCollisionEnter(Collision collision)
        {
            HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null && health.OwnerType == _target)
            {
                var force = (collision.gameObject.transform.position - this.gameObject.transform.position).normalized;
                health.DoKick(force);
            }
        } 
    }
}