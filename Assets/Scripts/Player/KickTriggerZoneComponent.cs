using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class KickTriggerZoneComponent: EnemyTriggerComponent
    {
        public void DoKick()
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy?.GetComponent<HealthComponent>();
                if (health != null)
                {
                    var force = Vector3.left;
                    health.DoKick(force);
                }
            }
        }
    }
}