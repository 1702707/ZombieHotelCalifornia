using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    [CreateAssetMenu(menuName = "Game/KillEnemyEvent")]
    public class KillEnemyEvent: ScriptableObject
    {
        public UnityEvent<ContactPoint> onEventTriggered;
        List<KillEnemyListener> listeners = new List<KillEnemyListener>();

        public void TriggerEvent(DamageData point) {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventTriggered(point);
        }

        public void AddListener(KillEnemyListener listener) {
            listeners.Add(listener);
        }

        public void RemoveListener(KillEnemyListener listener) {
            listeners.Remove(listener);
        }
    }
}