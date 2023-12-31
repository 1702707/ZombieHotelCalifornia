using System.Collections.Generic;
using UnityEngine;

namespace Controller.Components.Events
{
    public class BaseComboEvent: ScriptableObject
    {
        List<IListener<DamageData>> listeners = new List<IListener<DamageData>>();

        public void TriggerEvent(DamageData point) {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventTriggered(point);
        }

        public void AddListener(IListener<DamageData> listener) {
            listeners.Add(listener);
        }

        public void RemoveListener(IListener<DamageData> listener) {
            listeners.Remove(listener);
        }
    }
}