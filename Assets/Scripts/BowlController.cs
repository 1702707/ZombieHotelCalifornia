using System.Collections.Generic;
using System.Linq;
using Controller;
using Controller.Components;
using UnityEngine;

public class BowlController : BaseController
{
    [SerializeField] private InputController _inputController;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _spawnPoint; 
    [SerializeField] private int _prewarmCount;

    private List<IBallComponent> _pool = new List<IBallComponent>();
    private IBallComponent _currentBall;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        PrewarmPool();
        SetupBall();
        _inputController.SubscribeOnInput(OnInput);
    }

    private void OnInput(InputData data)
    {
        if (_currentBall == null)
        {
            SetupBall();
        }
        
        var ball = _currentBall;
        ball?.Push(data);
        ball?.SubscribeOnLifetimeEnd(() =>
        {
            ball.Release();
            _pool.Add(ball);
            SetupBall();
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
            _pool.Remove(go);
            _currentBall = go;
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
