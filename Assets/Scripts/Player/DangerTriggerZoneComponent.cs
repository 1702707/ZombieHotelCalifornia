using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.Player
{
    public class DangerTriggerZoneComponent: EnemyTriggerComponent
    {
        [SerializeField] private BoxCollider _box;
        [SerializeField] private Graphic _dangerZone;
        [SerializeField] private CapsuleCollider _player;

        private void Awake()
        {
            SetTransparent();
        }

        private void Update()
        {
            if (Enemies.Count > 0)
            {
                var closest = Enemies.Values.Max(go =>  go.transform.position.x);
                var color = _dangerZone.color;
                color.a = (closest + _box.bounds.size.x - _player.transform.position.x)/_box.bounds.size.x;
                _dangerZone.color = color;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (Enemies.Count == 0)
            {
                SetTransparent();
            }
        }

        private void SetTransparent()
        {
            var color = _dangerZone.color;
            color.a = 0;
            _dangerZone.color = color;
        }
    }
}