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

        public void DoKick(int Id)
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy.Value;
                var point = enemy.Value.transform.position;
                var data = new DamageData
                {
                    HitPoint = new Vector3(point.x,enemy.Value.Height,point.z),
                    Target = enemy.Value.gameObject,
                    Impulse = Vector3.forward,
                    SourceID = Id
                };
                if (health != null && health.OwnerType == _target)
                {
                    var force = _kickForce * Vector3.left;
                    
                    if (health.Staggered.InProgress)
                    {
                       health.DoKick(force, ()=>{health.DoDamage(data, _damage);}); 
                       health.Staggered.SetOnHeatAction((collision)=>
                       {
                           var other = collision.gameObject.GetComponent<HealthComponent>();
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