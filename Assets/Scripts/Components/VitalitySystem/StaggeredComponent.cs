using System;
using System.Collections;
using UnityEngine;

public class StaggeredComponent:MonoBehaviour
{
    private readonly float _staggeredTime = 2f;
    private IMovable _movable;
    private bool _inProgress;

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
}