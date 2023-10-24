using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.Components
{
    public class BallComponent : MonoBehaviour, IBallComponent
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCollider _collider;
        [SerializeField] private float _ballLifetime;
        [SerializeField] private MeshRenderer _ballRenderer;
        
        private List<Action> _listeners = new List<Action>();

        public void Push(InputData data)
        {
            _collider.isTrigger = false;
            _rigidbody.AddForce(data.ForceDirection, ForceMode.Force);
            _rigidbody.useGravity = true;
            StartCoroutine(Lifetime());
        }

        private IEnumerator Lifetime()
        {
            yield return new WaitForSeconds(_ballLifetime);
            OnLifetimeEnd();
        }

        private void OnLifetimeEnd()
        {
            foreach (var listener in _listeners)
            {
                listener.Invoke();
            }
            _listeners.Clear();
        }

        public void SubscribeOnLifetimeEnd(Action action)
        {
            _listeners.Add(action);
        }

        public void Activate()
        {
            this.gameObject.SetActive(true);
            _collider.isTrigger = true;
            // var mat = _ballRenderer.material;
            // mat.color = new Color(
            //     Random.Range(0, 255), 
            //     Random.Range(0, 255), 
            //     Random.Range(0, 255)
            // );
            // _ballRenderer.material = mat;
            _rigidbody.WakeUp();
        }

        public void Release()
        {
            gameObject.SetActive(false);
            _rigidbody.Sleep();
            _collider.isTrigger = true;
            _rigidbody.useGravity = false;
        }
    }

    public interface IBallComponent
    {
        void Activate();
        void Release();
        void Push(InputData data);
        void SubscribeOnLifetimeEnd(Action action);

        GameObject gameObject { get; }
    }

    
}