using System;
using System.Collections;
using Controller.Components.VitalitySystem;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Controller.Components.ComboController
{
    public class ComboController: MonoBehaviour
    {
        [Header("HeadShot")]
        [SerializeField] private GameObject _headshotPrefab;

        [FormerlySerializedAs("_headshotTime")] [SerializeField]
        private float _headshotDelay = 0.5f;

        private IObjectPool<GameObject> _headshotPool;

        private int _headshotCount;

        private void Awake()
        {
            _headshotPool = new LinkedPool<GameObject>(CreateHeadshotItem, OnGetItem, ReleaseItem, null, false, 1);
        }

        private void OnGetItem(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void ReleaseItem(GameObject obj)
        {
            obj.SetActive(false);
        }

        private GameObject CreateHeadshotItem()
        {
            return Instantiate(_headshotPrefab);
        }

        public void OnHeadshot(ContactPoint contact)
        {
            var effect = _headshotPool.Get();
            effect.transform.position = new Vector3(contact.point.x, contact.point.y + 0.5f, contact.point.z);
            //effect.transform.rotation = Quaternion.FromToRotation(Vector3.left, contact.normal);
            StartCoroutine(ReturnToPoolWithDelay(effect, _headshotPool, _headshotDelay));
            _headshotCount++;
        }

        private IEnumerator ReturnToPoolWithDelay<T>(T effect, IObjectPool<T> pool, float delay) where T : class
        {
            yield return new WaitForSeconds(delay);
            pool.Release(effect);
        }
    }

    [Serializable]
    public class EffectData
    {
        public EffectType Type;
        public GameObject Prefab;
    }
    
    [Serializable]
    public enum EffectType
    {
        Headshot,
    }
}