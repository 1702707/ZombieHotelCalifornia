using Controller.Components.VitalitySystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Controller.Player
{
    public class KickTriggerZoneComponent: EnemyTriggerComponent
    {
        [SerializeField] private int _kickForce;
        [SerializeField] private int _damage;
        [SerializeField] private float _delay;
        public float Delay => _delay;

        public void DoKick()
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy.Value;
                if (health != null && health.OwnerType == _target)
                {
                    var force = _kickForce * Vector3.left;
                    
                    if (enemy.Value.Staggered.InProgress)
                    {
                       health.DoKick(force); 
                       health.Staggered.SetOnHeatAction((collision)=>
                       {
                           var other = collision.gameObject.GetComponent<HealthComponent>();
                           other.DoDamage(_damage);
                           health.DoDamage(_damage);
                       });
                    }
                    else
                    {
                        health.DoDamage(_damage);
                    }
                }
            }
        }
    }
}