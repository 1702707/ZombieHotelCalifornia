using System;
using Controller.Components.VitalitySystem;
using Controller.Player;
using UnityEngine;

public class PlayerController : HealthComponent
{
    [SerializeField] private DamageTriggerZoneComponent _rightArm;
    [SerializeField] private DamageTriggerZoneComponent _leftArm;
    [SerializeField] private KickTriggerZoneComponent _rightLeg;
    [SerializeField] private KickTriggerZoneComponent _leftLeg;

    void Update()
    {
        if (Input.GetButton("LeftPunch"))
        {
            _leftArm.DoDamage();
        }
        
        if (Input.GetButton("RightPunch"))
        {
            _rightArm.DoDamage();
        }
        
        if (Input.GetButton("LeftKick"))
        {
            _leftLeg.DoKick();
        }
        
        if (Input.GetButton("RightKick"))
        {
            _rightLeg.DoKick();
        }
    }

    protected override void OnDamage()
    {
        if (CurrentHP == 0)
        {
            Debug.Log("You LOOSE!");
        }
    }

    protected override void OnKick(Vector3 force)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var health = other.GetComponent<DoDamageComponent>();
        if (health != null && health.Target == EntityType.Player)
        {
            DoDamage(health.Damage);
        }
    }
}
