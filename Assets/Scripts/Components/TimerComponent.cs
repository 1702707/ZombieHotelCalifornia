using System;
using TMPro;
using UnityEngine;

public class TimerComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    
    private long _startTicks;
    private long _totalTicks;
    private double _totalSeconds;

    // Start is called before the first frame update
    void Start()
    {
        _startTicks = DateTime.Now.Ticks;
    }

    // Update is called once per frame
    void Update()
    {
        _totalTicks = DateTime.Now.Ticks - _startTicks;
        var timeSpan = new TimeSpan(_totalTicks);
        _totalSeconds = timeSpan.TotalSeconds;
        _timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public double GetSessionDuration()
    {
        return _totalSeconds;
    }
}
