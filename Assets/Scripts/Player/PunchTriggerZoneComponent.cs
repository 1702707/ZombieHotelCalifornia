using System;
using System.Collections.Generic;
using Controller.Components.Events;
using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class PunchTriggerZoneComponent: EnemyTriggerComponent
    {
        [SerializeField]
        private float _delay;
        [SerializeField]
        private int _kickForce;

        public float Delay => _delay;

        public void DoPunch(int i)
        {
            foreach (var pair in Enemies)
            {
                var health = pair.Value;
                if (health.Staggered.InProgress)
                {
                    var pos = health.transform.position;
                    var data = new DamageData
                    {
                        Type = ComboType.Knock,
                        HitPoint = new Vector3(pos.x, health.Height / 2f, pos.z),
                        Height = health.Height,
                        Target = health.gameObject,
                        Impulse = _kickForce * Vector3.left,
                        SourceID = 0
                    };
                    var count = 0;
                    health.Staggered.SetOnHeatAction((collision)=>
                    {
                        var otherHealth = collision.gameObject.GetComponent<HealthComponent>();
                        var otherPos = otherHealth.transform.position;
                        var otherData = new DamageData
                        {
                            Type = ComboType.None,
                            HitPoint = new Vector3(otherPos.x, otherHealth.Height / 2f, otherPos.z),
                            Height = otherHealth.Height,
                            Target = otherHealth.gameObject,
                            Impulse = _kickForce * Vector3.left,
                            SourceID = 0
                        };
                        otherHealth.DoKick(otherData, null);
                        otherHealth.DoPunch();
                        count++;
                    });
                    health.DoKick(data, () =>
                    {
                        if (count > 0)
                        {
                            data.Type = ComboType.Collateral;
                            health.DoDamage(data, Int32.MaxValue);
                        }
                    });
                }
                else
                {
                    health.DoPunch();
                }
            }
        }
    }
    
    public enum PunchStage
    {
        Stagger,
        KnockBack
    }
}