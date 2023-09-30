using System;
using System.Collections.Generic;
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

        public void DoPunch()
        {
            foreach (var pair in Enemies)
            {
                var health = pair.Value;
                if (health.Staggered.InProgress)
                {
                    health.Staggered.SetOnHeatAction((collision)=>
                    {
                        var otherHealth = collision.gameObject.GetComponent<HealthComponent>();
                        otherHealth.DoKick(_kickForce * Vector3.left, null);
                        otherHealth.DoPunch();
                    });
                    health.DoKick(_kickForce * Vector3.left, null);
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