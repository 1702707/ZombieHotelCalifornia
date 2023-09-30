using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    public class HeadshotListener : MonoBehaviour {
        public HeadshotEvent gameEvent;
        public UnityEvent<ContactPoint> onEventTriggered;

        void OnEnable() {
            gameEvent.AddListener(this);
        }

        void OnDisable() {
            gameEvent.RemoveListener(this);
        }

        public void OnEventTriggered(ContactPoint contactPoint)
        {
            onEventTriggered.Invoke(contactPoint);
        }
    }
}