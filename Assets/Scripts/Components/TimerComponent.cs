using System;
using TMPro;
using UnityEngine;

public class TimerComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    
    private long _startTicks;
    private long _totalTicks;
    private double _totalSeconds;
    private TimeSpan _timeSpan;

    // Start is called before the first frame update
    void Start()
    {
        _startTicks = DateTime.Now.Ticks;
    }

    // Update is called once per frame
    void Update()
    {
        _totalTicks = DateTime.Now.Ticks - _startTicks;
        _timeSpan = new TimeSpan(_totalTicks);
        _totalSeconds = _timeSpan.TotalSeconds;
        _timerText.text = $"{_timeSpan.Minutes:D2}:{_timeSpan.Seconds:D2}";
    }

    public TimeSpan GetSessionDuration()
    {
        return _timeSpan;
    }
}
