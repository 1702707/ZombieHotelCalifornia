using System;
using UnityEngine;

namespace Controller
{
    public class BaseController: MonoBehaviour
    {
        protected virtual void OnInitialize(){}
        protected virtual void OnDispose(){}

        private void OnEnable()
        {
            OnInitialize();
        }

        private void OnDestroy()
        {
            OnDispose();
        }
    }
}