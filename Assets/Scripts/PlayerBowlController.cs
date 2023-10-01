using System.Collections.Generic;
using System.Linq;
using Controller;
using Controller.Components;
using UnityEngine;

public class PlayerBowlController : BaseController
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _spawnPoint; 
    [SerializeField] private int _prewarmCount;

    private List<IBallComponent> _pool = new List<IBallComponent>();
    private IBallComponent _currentBall;
    private int _ballID = 0;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        PrewarmPool();
        SetupBall();
        _inputController.SubscribeOnInput(OnInput);
        _inputController.SubscribeOnStartInput(OnStartInput);
    }

    private void OnStartInput()
    {
        ShowBall();
        _animator.SetBool("Measure", true);
    }

    public void ShowBall()
    {
        if (_currentBall == null)
        {
            SetupBall();
        }
        else
        {
            _currentBall.Activate();
        }
    }

    public void HideBall()
    {
        _currentBall?.Release();
    }

    private void OnInput(InputData data)
    {
        var ball = _currentBall;
        ball?.Push(data);
        _animator.SetBool("Measure", false);
        _animator.SetTrigger("ThrowBall");
        ball?.SubscribeOnLifetimeEnd(() =>
        {
            ball.Release();
            _pool.Add(ball);
        });
        _currentBall = null;
    }

    private void SetupBall()
    {
        if (_pool.Count > 0)
        {
            if(_currentBall != null)
                return;
            
            var go = _pool.FirstOrDefault();
            go.Activate();
            go.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            go.gameObject.name = _ballID.ToString();
            _pool.Remove(go);
            _currentBall = go;
            _ballID++;
        }
        else
        {
            PrewarmPool();
            SetupBall();
            Debug.Log("Loop");
        }
    }

    private void PrewarmPool()
    {
        for (var i = 0; i < _prewarmCount; i++)
        {
            var go = GameObject.Instantiate(_ballPrefab, _spawnPoint).GetComponent<BallComponent>();
            go.gameObject.SetActive(false);
            _pool.Add(go);
        }
    }
}
