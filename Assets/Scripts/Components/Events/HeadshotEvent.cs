using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    [CreateAssetMenu(menuName = "Game/HeadShotEvent")]
    public class HeadshotEvent: ScriptableObject 
    {
        List<HeadshotListener> listeners = new List<HeadshotListener>();

        public void TriggerEvent(DamageData point) {
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