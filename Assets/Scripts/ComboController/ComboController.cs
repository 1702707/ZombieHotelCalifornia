using System;
using System.Collections;
using System.Collections.Generic;
using Controller.Components.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace Controller.Components.ComboController
{
    public class ComboController: MonoBehaviour
    {
        [Header("Data")] 
        [SerializeField] private ComboDataScriptableObject _comboData;
        
        [Header("Prefabs")]
        [SerializeField] private SpriteRenderer _comboPrefab;
        [SerializeField] private Transform _damagePrefab;
        
        [Header("Combo")]
        [SerializeField] private float _comboDelay = 0.5f;
        [SerializeField] private float _damageDelay = 0.5f;

        [FormerlySerializedAs("_scoreText")]
        [Header("Score")]
        [SerializeField] private ScoreComponent _scoreComponent;
        [SerializeField] private TimerComponent _timerComponent;
        [SerializeField] private TMP_Text _highScoreText;

        private const string Highscore = "HighScore";
        private const string Time = "Time";
        private const string LastScore = "PreviousScore";
        
        private int _headshotCount;
        private int _totalKillCount;
        private int _totalScore;
        
        private IObjectPool<SpriteRenderer> _itemPool;
        private IObjectPool<Transform> _damagePool;

        private Dictionary<int, int> _comboCounter = new Dictionary<int, int>();
        private int _highScore;

        private void Awake()
        {
            _itemPool = new LinkedPool<SpriteRenderer>(()=>CreateItem(_comboPrefab), OnGetItem, ReleaseItem, null, false, 20);
            _damagePool = new LinkedPool<Transform>(() => CreateItem(_damagePrefab), OnGetItem, ReleaseItem, null, false, 20);
            _highScore = PlayerPrefs.GetInt(Highscore);
            _highScoreText.text = _highScore.ToString("000000000");
        }

        private T CreateItem<T>(T prefab) where T:Component
        {
            return Instantiate(prefab);
        }
        
        private void OnGetItem<T>(T obj) where T:Component
        {
            obj.gameObject.SetActive(true);
        }

        private void ReleaseItem<T>(T obj) where T: Component
        {
            obj.gameObject.SetActive(false);
        }

        public void OnHeadshot(DamageData contact)
        {
            var data = _comboData.GetData(ComboType.Headshot);
            var effect = _itemPool.Get();
            
            if (effect != null)
            {
                effect.sprite = data.Sprite;
                effect.transform.position = new Vector3(contact.HitPoint.x, contact.Height, contact.HitPoint.z);
                StartCoroutine(ReturnToPoolWithDelay(effect, _itemPool, _comboDelay));
            }
            
            IncreaseCounter(contact.SourceID);
            _headshotCount++;
            Score += data.Score;
        }
        
        public void OnDamage(DamageData contact)
        {
            var effect = _damagePool.Get();
            effect.transform.position = contact.HitPoint;
            StartCoroutine(ReturnToPoolWithDelay(effect, _damagePool, _damageDelay));
            Score += _comboData.DamageScore;
        }
        
        public void OnKill(DamageData data)
        {
            var count = IncreaseCounter(data.SourceID);

            Debug.Log(data.SourceID + " "+count);
            var combo = _comboData.GetData(count.ToComboType());

            if (combo == null)
            {
                Debug.LogError($"CAUTION! Combo == null count - {count} type - {count.ToComboType()}");
                return;
            }
            
            if (count > 1)
            {
               SpriteRenderer effect = _itemPool.Get();
               if (effect != null)
               {
                   effect.sprite = combo.Sprite;
                   effect.transform.position = new Vector3(data.HitPoint.x, data.Height, data.HitPoint.z);
                   StartCoroutine(ReturnToPoolWithDelay(effect, _itemPool, _comboDelay));
               }
               else
               {
                   Debug.LogError("CAUTION! Effect = null");
               }
            }
            
            Score += combo.Score;
            _totalKillCount++; 
        }
        
        public void OnKnock(DamageData data)
        {
            var combo = _comboData.GetData(ComboType.Knock);

            if (combo == null)
            {
                Debug.LogError($"CAUTION! Combo == null type - Knock");
                return;
            }
            
            
            SpriteRenderer effect = _itemPool.Get();
            if (effect != null)
            {
                effect.sprite = combo.Sprite;
                effect.transform.position = data.HitPoint;
                StartCoroutine(ReturnToPoolWithDelay(effect, _itemPool, _comboDelay));
            }
            else
            {
                Debug.LogError("CAUTION! Effect = null");
            }
            
            Score += combo.Score;
        }

        public void OnGameOver(DamageData data)
        {
            if (Score > _highScore)
            {
                PlayerPrefs.SetInt(Highscore, Score);
                PlayerPrefs.SetInt(Time, _timerComponent.GetSessionDuration());
            }
            PlayerPrefs.SetInt(LastScore, Score);
        }

        private int IncreaseCounter(int id)
        {
            if (_comboCounter.ContainsKey(id))
            {
                _comboCounter[id]++;
            }
            else
            {
                _comboCounter.Add(id,1);
            }
            return _comboCounter[id];
        }

        private int Score
        {
            get => _totalScore;
            set
            {
                _totalScore = value;
                _scoreComponent.SetScore(_totalScore);
            }
        }

        private IEnumerator ReturnToPoolWithDelay<T>(T effect, IObjectPool<T> pool, float delay) where T : class
        {
            yield return new WaitForSeconds(delay);
            pool.Release(effect);
        }
    }
}

[Serializable]
public class ComboData
{
    public ComboType Type;
    public Sprite Sprite;
    public int Score;
}

[Serializable]
public enum ComboType
{
    Headshot = 0,
    Kill = 1,
    DoubleKill = 2,
    TriplrKill = 3,
    QuadroKill = 4,
    PentaKill = 5,
    MegaKill,
    Strike,
    Stagger,
    Knock
}

public static class ComboTypeEx
{
    public static ComboType ToComboType(this int count)
    {
        switch (count)
        {
            case 1:
                return ComboType.Kill;
            case 2:
                return ComboType.DoubleKill;
            case 3:
                return ComboType.TriplrKill;
            case 4:
                return ComboType.QuadroKill;
            case 5:
                return ComboType.PentaKill;
            default:
                return ComboType.MegaKill;
        }
    }
}



