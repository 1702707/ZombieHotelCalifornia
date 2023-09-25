using System;
using Controller.Components.VitalitySystem;
using Controller.Player;
using UnityEngine;

public class PlayerController : HealthComponent
{
    [SerializeField] private PunchTriggerZoneComponent _rightArm;
    [SerializeField] private PunchTriggerZoneComponent _leftArm;
    [SerializeField] private KickTriggerZoneComponent _rightLeg;
    [SerializeField] private KickTriggerZoneComponent _leftLeg;

    void Update()
    {
        if (Input.GetButtonUp("LeftPunch"))
        {
            _leftArm.DoPunch();
        }
        
        if (Input.GetButtonUp("RightPunch"))
        {
            Debug.Log("OnPunch");
            _rightArm.DoPunch();
        }
        
        if (Input.GetButtonUp("LeftKick"))
        {
            _leftLeg.DoKick();
        }
        
        if (Input.GetButtonUp("RightKick"))
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

    protected override void onPunch()
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
