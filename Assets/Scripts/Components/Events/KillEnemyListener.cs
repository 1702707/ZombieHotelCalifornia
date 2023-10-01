using UnityEngine;
using UnityEngine.Events;

namespace Controller.Components.Events
{
    public class KillEnemyListener: MonoBehaviour
    {
        public KillEnemyEvent gameEvent;
        public UnityEvent<DamageData> onEventTriggered;

        void OnEnable() {
            gameEvent.AddListener(this);
        }

        void OnDisable() {
            gameEvent.RemoveListener(this);
        }

        public void OnEventTriggered(DamageData contactPoint)
        {
            onEventTriggered.Invoke(contactPoint);
        }
    }
}