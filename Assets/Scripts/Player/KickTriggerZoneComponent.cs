using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class KickTriggerZoneComponent: EnemyTriggerComponent
    {
        [SerializeField] private int _kickForce;
        [SerializeField] private int _damage;
        public void DoKick()
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy.Value.GetComponent<HealthComponent>();
                if (health != null && health.OwnerType == _target)
                {
                    var force = _kickForce * Vector3.left;
                    health.DoKick(force);
                    var staggered = enemy.Value.GetComponent<StaggeredComponent>();
                    if (staggered != null && staggered.InProgress)
                    {
                        health.DoDamage(_damage);
                    }
                }
            }
        }
    }
}