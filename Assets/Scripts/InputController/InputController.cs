using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private float _maxStrenge;

    private Vector2 _startPoint;
    private float _maxDistance;
    private Vector2 _directionStartPoint;
    private List<Action<InputData>> _listeners = new List<Action<InputData>>();
    private Vector2 _directionEndPoint;
    private bool _forceMeasure;


    private void Update()
    {
        Debug.DrawLine(_startPoint,_directionStartPoint, Color.red);
        Debug.DrawLine(_directionStartPoint, _directionEndPoint, Color.blue);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_forceMeasure && _directionStartPoint.y > eventData.position.y)
        {
            var distance = Vector2.Distance(_startPoint, eventData.position);
            if (_maxDistance <= distance)
            {
                _maxDistance = distance;
                _directionStartPoint = eventData.position;
            }
            else
            {
                _forceMeasure = false;
            }
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPoint = eventData.position;
        _directionStartPoint = Vector2.positiveInfinity;
        _directionEndPoint = Vector2.negativeInfinity;
        _maxDistance = 0;
        _forceMeasure = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _directionEndPoint = eventData.position;
        FireEvent();
    }

    private void FireEvent()
    {
        var force = _maxDistance;
        var direction = (_directionEndPoint - _directionStartPoint).normalized;
        var data = new InputData()
        {
            Force = force,
            Direction = direction,
            ForceDirection =  force * new Vector3(-1, direction.y, direction.x)
        };
        foreach (var listener in _listeners)
        {
            listener.Invoke(data);
        }
    }

    public void SubscribeOnInput(Action<InputData> action)
    {
        _listeners.Add(action);
    }

    private void OnDestroy()
    {
        _listeners.Clear();
    }
}

public class InputData
{
    public float Force { get; set; }
    public Vector2 Direction { get; set; }
    
    public Vector3 ForceDirection { get; set; }
}
