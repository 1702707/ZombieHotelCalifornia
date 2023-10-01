using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    public class DamageListener: MonoBehaviour, IListener<DamageData>
    {
        public DamageEvent gameEvent;
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