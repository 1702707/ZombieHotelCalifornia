using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class KickTriggerZoneComponent: EnemyTriggerComponent
    {
        [SerializeField] private EntityType _target;
        public void DoKick()
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy?.GetComponent<HealthComponent>();
                if (health != null && health.OwnerType == _target)
                {
                    var force = Vector3.left;
                    health.DoKick(force);
                }
            }
        }
    }
}