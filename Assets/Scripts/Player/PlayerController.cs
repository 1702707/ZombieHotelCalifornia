using System;
using System.Collections;
using Controller.Components.ComboController;
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
    private int _actionId;

    [SerializeField] private AudioClip punchAudio;
    [SerializeField] private AudioClip kickAudio;

    private void Update()
    {
        var now = new TimeSpan(DateTime.Now.Ticks).TotalSeconds;
        if (now < _expDelaySeconds)
            return;

        if (Input.GetButtonUp("LeftPunch"))
        {
            _expDelaySeconds = now + _leftArm.Delay;
            _animator.SetTrigger("LeftPunch");
            StartCoroutine(DoActionWithDelay(_leftArm.Delay, _leftArm.DoPunch,punchAudio));
        }

        if (Input.GetButtonUp("RightPunch"))
        {
            _expDelaySeconds = now + _rightArm.Delay;
            _animator.SetTrigger("RightPunch");
            StartCoroutine(DoActionWithDelay(_rightArm.Delay, _rightArm.DoPunch, punchAudio));
        }

        if (Input.GetButtonUp("LeftKick"))
        {
            _expDelaySeconds = now + _leftLeg.Delay;
            _animator.SetTrigger("LeftKick");
            StartCoroutine(DoActionWithDelay(_leftLeg.Delay, _leftLeg.DoKick, kickAudio));
        }

        if (Input.GetButtonUp("RightKick"))
        {
            _expDelaySeconds = now + _rightLeg.Delay;
            _animator.SetTrigger("RightKick");
            StartCoroutine(DoActionWithDelay(_rightLeg.Delay, _rightLeg.DoKick, kickAudio));
        }
    }

    private IEnumerator DoActionWithDelay(float delay, Action<int> doPunch, AudioClip clip)
    {
        yield return new WaitForSeconds(delay);
        _actionId = IDGenerator.Get();
        if(clip != null ) 
        {
            AudioManager.Instance.PlaySound(clip);
        }
        doPunch.Invoke(_actionId);
    }

    protected override void OnDamage()
    {
    }

    protected override void OnKick(Vector3 force, Action callback)
    {
    }

    protected override void OnDeath()
    {
    }

    protected override void OnHeadshot()
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