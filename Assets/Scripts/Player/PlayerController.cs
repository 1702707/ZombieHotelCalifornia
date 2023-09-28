using System;
using System.Collections;
using Controller.Components.VitalitySystem;
using Controller.Player;
using UnityEngine;

public class PlayerController : HealthComponent
{
    [SerializeField] private PunchTriggerZoneComponent _rightArm;
    [SerializeField] private PunchTriggerZoneComponent _leftArm;
    [SerializeField] private KickTriggerZoneComponent _rightLeg;
    [SerializeField] private KickTriggerZoneComponent _leftLeg;
    [SerializeField] private Animator _animator;
    
    private float _currentDelay;
    private double _expDelaySeconds;

    void Update()
    {
        if(DateTime.Now.Second > _expDelaySeconds)
            
        if (Input.GetButtonUp("LeftPunch"))
        {
            _expDelaySeconds = DateTime.Now.Second + _leftArm.Delay;
            _animator.SetTrigger("LeftPunch");
            StartCoroutine(DoActionWithDelay(_leftArm.Delay, _leftArm.DoPunch));
        }
        
        if (Input.GetButtonUp("RightPunch"))
        {
            _expDelaySeconds = DateTime.Now.Second + _rightArm.Delay;
            _animator.SetTrigger("RightPunch");
            StartCoroutine(DoActionWithDelay(_rightArm.Delay, _rightArm.DoPunch));
        }
        
        if (Input.GetButtonUp("LeftKick"))
        {
            _expDelaySeconds = DateTime.Now.Second + _leftLeg.Delay;
            StartCoroutine(DoActionWithDelay(_leftLeg.Delay, _leftLeg.DoKick));
        }
        
        if (Input.GetButtonUp("RightKick"))
        {
            _expDelaySeconds = DateTime.Now.Second + _rightLeg.Delay;
            StartCoroutine(DoActionWithDelay(_rightLeg.Delay, _rightLeg.DoKick));
        }
    }

    private IEnumerator DoActionWithDelay(float delay, Action doPunch)
    {
        yield return new WaitForSeconds(delay);
        doPunch.Invoke();
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

    // private void OnTriggerEnter(Collider other)
    // {
    //     var health = other.GetComponent<DoDamageComponent>();
    //     if (health != null && health.Target == EntityType.Player)
    //     {
    //         DoDamage(health.Damage);
    //     }
    // }
}
