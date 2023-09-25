using System.Collections.Generic;
using System.Linq;
using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class EnemyTriggerComponent: MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private float _castTime;
        [SerializeField] private float _delayTime;
        [SerializeField] protected EntityType _target;

        protected Dictionary<int, HealthComponent> Enemies
        {
            get
            {
                foreach (var pair in _enemies.Where(pair => pair.Value == null))
                {
                    _enemies.Remove(pair.Key);
                }

                return _enemies;
            }
        }

        private Dictionary<int, HealthComponent> _enemies = new Dictionary<int, HealthComponent>();

        private void OnTriggerEnter(Collider other)
        {
            var health = other.GetComponent<HealthComponent>();
            if(health != null && health.OwnerType == _target)
                _enemies[other.GetHashCode()] = health;
        }

        private void OnTriggerExit(Collider other)
        {
            var id = other.GetHashCode();
            if (_enemies.ContainsKey(id))
                _enemies.Remove(id);
        }
    }
}