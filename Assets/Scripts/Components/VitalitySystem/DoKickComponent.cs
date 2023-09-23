using UnityEngine;

namespace Controller.Components.VitalitySystem
{
    public class DoKickComponent: MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            HealthComponent health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null)
            {
                var force = (collision.gameObject.transform.position - this.gameObject.transform.position).normalized;
                health.DoKick(force);
            }
        } 
    }
}