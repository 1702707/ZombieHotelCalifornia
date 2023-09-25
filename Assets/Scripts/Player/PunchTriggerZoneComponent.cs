using System;
using System.Collections.Generic;
using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class PunchTriggerZoneComponent: EnemyTriggerComponent
    {
        [SerializeField]
        private int _kickForce;
        public void DoPunch()
        {
            foreach (var pair in Enemies)
            {
                var staggered = pair.Value.GetComponent<StaggeredComponent>();
                if (staggered == null)
                {
                    staggered = pair.Value.gameObject.AddComponent<StaggeredComponent>();
                }

                if (staggered.InProgress)
                {
                    pair.Value.DoKick(_kickForce * Vector3.left);
                }
                else
                {
                    staggered.Do();
                    pair.Value.DoPunch();
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