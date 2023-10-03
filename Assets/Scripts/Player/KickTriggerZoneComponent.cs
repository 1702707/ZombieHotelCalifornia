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
        private ComboEvent _knockEvent;
        public float Delay => _delay;

        public void DoKick(int Id)
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy.Value;
                if (health != null && health.OwnerType == _target)
                {
                    var point = health.transform.position;
                    var data = new DamageData
                    {
                        Target = health.gameObject,
                        Impulse = health.toppleForce * Vector3.one,
                        SourceID = Id,
                        HitPoint = new Vector3(point.x, health.Height / 2f, point.z),
                        Height = health.Height,
                        Type = ComboType.Damage
                    };
               
                    if (health.Staggered.InProgress)
                    {
                        var launch = data;
                        launch.Type = ComboType.Launch;
                        launch.Impulse = _kickForce * Vector3.left;
                        health.DoKick(launch, 
                            () =>
                            {
                                var smash = data;
                                smash.Type = ComboType.Smash;
                                health.DoDamage(smash, _damage);
                            }); 
                        health.Staggered.SetOnHeatAction(collision=>
                        {
                           var other = collision.gameObject.GetComponent<HealthComponent>();
                           var otherPos = other.transform.position;
                           var otherData = new DamageData
                           {
                               Type = ComboType.Smash,
                               HitPoint = new Vector3(otherPos.x, other.Height / 2f, otherPos.z),
                               Height = other.Height,
                               Target = other.gameObject,
                               Impulse = other.toppleForce * Vector3.one,
                               SourceID = Id
                           };
                           other.DoDamage(otherData, _damage);
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