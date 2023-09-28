using System;
using System.Collections;
using Controller.Components.VitalitySystem;
using UnityEngine;

public class StaggeredComponent:MonoBehaviour
{
    private readonly float _staggeredTime = 2f;
    private IMovable _movable;
    private bool _inProgress;
    private Action<Collision> _onHitAction;

    public bool InProgress => _inProgress;

    private void Awake()
    {
        _inProgress = false;
        _movable = GetComponent<IMovable>();
    }

    public void Do()
    {
        StartCoroutine(StopMove());
    }

    private IEnumerator StopMove()
    {
        _inProgress = true;
        _movable.StopMove();

        yield return new WaitForSeconds(_staggeredTime);

        _inProgress = false;
        _movable.Move();
        this.enabled = false;
    }

    public void SetOnHeatAction(Action<Collision> action)
    {
        _onHitAction = action;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (InProgress && _onHitAction != null)
        {
            var health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null && health.OwnerType == EntityType.Enemy)
            {
                _onHitAction.Invoke(collision);
                _onHitAction = null;
            }
        }
    }
}