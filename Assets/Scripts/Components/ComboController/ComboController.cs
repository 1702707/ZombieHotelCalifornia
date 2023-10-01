using System;
using System.Collections;
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
        private int _totalKillCount;

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

        public void OnHeadshot(Vector3 contact)
        {
            var effect = _headshotPool.Get();
            effect.transform.position = new Vector3(contact.x, contact.y + 0.5f, contact.z);
            //effect.transform.rotation = Quaternion.FromToRotation(Vector3.left, contact.normal);
            StartCoroutine(ReturnToPoolWithDelay(effect, _headshotPool, _headshotDelay));
            _headshotCount++;
        }
        
        public void OnHeadshot(DamageData data)
        {
            // var effect = _headshotPool.Get();
            // effect.transform.position = new Vector3(data.HitPoint.x, data.HitPoint.y, data.HitPoint.z);
            // StartCoroutine(ReturnToPoolWithDelay(effect, _headshotPool, _headshotDelay));
            _totalKillCount++;
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