using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private float _maxStrenge;
    [SerializeField] private Transform _forceUI;

    private Vector2 _startPoint;
    private float _maxDistance;
    private Vector2 _directionStartPoint;
    private List<Action<InputData>> _listeners = new List<Action<InputData>>();
    private List<Action> _listenersStart = new List<Action>();
    private Vector2 _directionEndPoint;
    private bool _forceMeasure;
    private List<Image> _uiParts;

    private void Awake()
    {
        _uiParts = new List<Image>();
        for (var i = 0; i < _forceUI.childCount; i++)
        {
            var image = _forceUI.transform.GetChild(i).GetComponent<Image>();
            var imageColor = image.color;
            imageColor.a = 0;
            image.color = imageColor;
            _uiParts.Add(image);
        }
        
    }

    private void Update()
    {
        Debug.DrawLine(_startPoint,_directionStartPoint, Color.red);
        Debug.DrawLine(_directionStartPoint, _directionEndPoint, Color.blue);
        if (_forceMeasure)
        {
            var force = Mathf.Clamp(_maxDistance,1,_maxStrenge)/_maxStrenge;
            var visibleCount = Math.Round(force * _uiParts.Count);
            for (var index = 0; index < _uiParts.Count; index++)
            {
                var uiPart = _uiParts[index];
                var uiPartColor = uiPart.color;
                uiPartColor.a = index<visibleCount? 1: 0;
                uiPart.color = uiPartColor;
            }
        }
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
        foreach (var action in _listenersStart)
        {
            action.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _directionEndPoint = eventData.position;
        FireEvent();
    }

    private void FireEvent()
    {
        var force = Mathf.Clamp(_maxDistance,1,_maxStrenge);
        var direction = (_directionEndPoint - _directionStartPoint).normalized;
        var data = new InputData()
        {
            Force = force,
            Direction = direction,
            ForceDirection =  20*force * new Vector3(-1, Mathf.Max(direction.y), direction.x)
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
        _listenersStart.Clear();
    }

    public void SubscribeOnStartInput(Action onStartInput)
    {
        _listenersStart.Add(onStartInput);
    }
}

public class InputData
{
    public float Force { get; set; }
    public Vector2 Direction { get; set; }
    
    public Vector3 ForceDirection { get; set; }
}
