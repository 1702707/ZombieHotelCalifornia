using System;
using System.Collections.Generic;
using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class DamageTriggerZoneComponent: EnemyTriggerComponent
    {
        [SerializeField] private int _damage;
        [SerializeField] private EntityType _target;

        public void DoDamage()
        {
            foreach (var enemy in Enemies)
            {
                var health = enemy.GetComponent<HealthComponent>();
                if (health != null && health.OwnerType == _target)
                {
                    health.DoDamage(_damage);
                }
            }
        }
    }
}