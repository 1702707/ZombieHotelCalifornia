using System.Collections.Generic;
using System.Linq;
using Controller.Components.VitalitySystem;
using UnityEngine;

namespace Controller.Player
{
    public class EnemyTriggerComponent: MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] protected EntityType _target;

        protected Dictionary<int, HealthComponent> Enemies
        {
            get
            {
                var enemy = _enemies.Where(pair => pair.Value == null).ToList();
                foreach (var pair in enemy)
                {
                    _enemies.Remove(pair.Key);
                }

                return _enemies;
            }
        }

        private Dictionary<int, HealthComponent> _enemies = new Dictionary<int, HealthComponent>();

        private void OnTriggerEnter(Collider other)
        {
            var collisionPoint = other.ClosestPoint(transform.position);
            var health = other.GetComponent<HealthComponent>();
            if(health != null && health.OwnerType == _target)
                _enemies[other.gameObject.GetHashCode()] = health;
        }

        private void OnTriggerExit(Collider other)
        {
            var id = other.gameObject.GetHashCode();
            if (_enemies.ContainsKey(id))
                _enemies.Remove(id);
        }
    }
}