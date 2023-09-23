using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controller.Player
{
    public class EnemyTriggerComponent: MonoBehaviour
    {
        [SerializeField] private BoxCollider _collider;
        
        public List<Collider> Enemies
        {
            get
            {
                _enemies = _enemies.Where(enemy => enemy != null).ToList();
                return _enemies;
            }
        }

        private List<Collider> _enemies = new List<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            _enemies.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_enemies.Contains(other))
                _enemies.Remove(other);
        }
        
        //private List<Collision> _enemies = new List<Collision>();

        // private void OnCollisionEnter(Collision other)
        // {
        //     _enemies.Add(other);
        // }
        //
        // private void OnCollisionExit(Collision other)
        // {
        //     if (_enemies.Contains(other))
        //         _enemies.Remove(other);
        // }
    }
}