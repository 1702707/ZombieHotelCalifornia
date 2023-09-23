using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller.Components
{
    public class BallComponent : MonoBehaviour, IBallComponent
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _ballLifetime;
        
        private List<Action> _listeners = new List<Action>();

        public void Push(InputData data)
        {
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
            _rigidbody.WakeUp();
        }

        public void Release()
        {
            gameObject.SetActive(false);
            _rigidbody.Sleep();
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