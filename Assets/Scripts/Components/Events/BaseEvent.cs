using System.Collections.Generic;
using UnityEngine;

namespace Controller.Components.Events
{
    public class BaseEvent: ScriptableObject
    {
        List<IListener> listeners = new List<IListener>();

        public void TriggerEvent() {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventTriggered();
        }

        public void AddListener(IListener listener) {
            listeners.Add(listener);
        }

        public void RemoveListener(IListener listener) {
            listeners.Remove(listener);
        }
    }
}