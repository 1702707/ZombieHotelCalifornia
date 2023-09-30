using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    [CreateAssetMenu(menuName = "Game/HeadShot Event")]
    public class HeadshotEvent: ScriptableObject 
    {
        public UnityEvent<ContactPoint> onEventTriggered;
        List<HeadshotListener> listeners = new List<HeadshotListener>();

        public void TriggerEvent(ContactPoint point) {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventTriggered(point);
        }
    
        public void AddListener(HeadshotListener listener) {
            listeners.Add(listener);
        }

        public void RemoveListener(HeadshotListener listener) {
            listeners.Remove(listener);
        }
    }
}