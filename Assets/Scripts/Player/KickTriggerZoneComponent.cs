using Controller.Components.Events;
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

        [SerializeField]
        private KnockEvent _knockEvent;
        public float Delay => _delay;

        public void DoKick(int Id)
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy.Value;
                var point = enemy.Value.transform.position;
                var data = new DamageData
                {
                    Target = enemy.Value.gameObject,
                    Impulse = enemy.Value.toppleForce * Vector3.one,
                    SourceID = Id,
                    HitPoint = new Vector3(point.x, health.Height / 2f, point.z),
                    Height = health.Height
                };
                
                if (health != null && health.OwnerType == _target)
                {
                    var force = _kickForce * Vector3.left;
                    if (health.Staggered.InProgress)
                    { 
                        _knockEvent.TriggerEvent(data);
                        health.DoKick(force, () => {
                           data.HitPoint = new Vector3(health.transform.position.x, health.Height / 2f,
                               health.transform.position.z);
                           data.Height = health.Height;
                           health.DoDamage(data, _damage);
                        }); 
                        health.Staggered.SetOnHeatAction(collision=>
                        {
                           var other = collision.gameObject.GetComponent<HealthComponent>();
                           data.HitPoint = new Vector3(other.transform.position.x, other.Height / 2f,
                               other.transform.position.z);
                           data.Height = health.Height;
                           other.DoDamage(data, _damage);
                        });
                    }
                    else
                    {
                        health.DoDamage(data, _damage);
                    }
                }
            }
        }
    }
}