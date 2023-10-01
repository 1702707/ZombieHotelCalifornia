using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    public class BaseDamageDataListener<T>:MonoBehaviour,IListener<DamageData> where T:BaseDamageDataEvent
    {
        public T gameEvent;
        public UnityEvent<DamageData> onEventTriggered;

        public void OnEnable() {
            gameEvent.AddListener(this);
        }

        public void OnDisable() {
            gameEvent.RemoveListener(this);
        }

        public void OnEventTriggered(DamageData contactPoint)
        {
            onEventTriggered.Invoke(contactPoint);
        }
    }
}