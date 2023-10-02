using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    public class BaseListener<T>: MonoBehaviour,IListener where T:BaseEvent
    {
        public T gameEvent;
        public UnityEvent onEventTriggered;

        public void OnEnable() {
            gameEvent.AddListener(this);
        }

        public void OnDisable() {
            gameEvent.RemoveListener(this);
        }

        public void OnEventTriggered()
        {
            onEventTriggered.Invoke();
        }
    }
}