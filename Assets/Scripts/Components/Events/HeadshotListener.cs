using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    public class HeadshotListener : MonoBehaviour 
    {
        public HeadshotEvent gameEvent;
        public UnityEvent<Vector3> onEventTriggered;

        void OnEnable() {
            gameEvent.AddListener(this);
        }

        void OnDisable() {
            gameEvent.RemoveListener(this);
        }

        public void OnEventTriggered(Vector3 contactPoint)
        {
            onEventTriggered.Invoke(contactPoint);
        }
    }
}