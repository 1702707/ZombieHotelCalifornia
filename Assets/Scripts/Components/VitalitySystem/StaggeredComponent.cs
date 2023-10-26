using System;
using System.Collections;
using Controller.Components.VitalitySystem;
using UnityEngine;

public class StaggeredComponent:MonoBehaviour
{
    [SerializeField] private GameObject _staggeredEffect;
    
    private readonly float _staggeredTime = 2f;
    private IMovable _movable;
    private bool _inProgress;
    private Action<Collision> _onHitAction;

    public bool InProgress => _inProgress;

    private void Awake()
    {
        _inProgress = false;
        _movable = GetComponent<IMovable>();
        _staggeredEffect.SetActive(false);
    }

    public void Do()
    {
        StartCoroutine(StopMove());
    }

    private IEnumerator StopMove()
    {
        _inProgress = true;
        _movable.StopMove();
        _staggeredEffect.SetActive(true);

        yield return new WaitForSeconds(_staggeredTime);

        _inProgress = false;
        _movable.Move();
        _staggeredEffect.SetActive(false);
    }

    public void SetOnHeatAction(Action<Collision> action)
    {
        _onHitAction = action;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Staggred {collision.gameObject.name}");
        if ( _onHitAction != null)
        {
            var health = collision.gameObject.GetComponent<HealthComponent>();
            if (health != null && health.OwnerType == EntityType.Enemy)
            {
                Debug.Log($"Staggred {gameObject.name} hits {health.gameObject.name}");
                _onHitAction.Invoke(collision);
                _onHitAction = null;
            }
        }
    }
}