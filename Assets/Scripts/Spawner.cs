using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float Countdown;

    //Preset formation waves
    [SerializeField] private Wave[] waves;
    [SerializeField] private int waveToSpawn = 0;

    [Header("Random Wave Waypoints")]
    [SerializeField] private Zombie prefab;
    [SerializeField] private Zombie crawler;
    [SerializeField] private float randomGapTime;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] randomFrontLeftSpawnPoints;
    [SerializeField] private Transform[] randomFrontRightSpawnPoints;
    [SerializeField] private Transform[] randomMidLeftSpawnPoints;
    [SerializeField] private Transform[] randomMidRightSpawnPoints;
    [SerializeField] private Transform[] randomBackLeftSpawnPoints;
    [SerializeField] private Transform[] randomBackRightSpawnPoints;

    // Update is called once per frame
    void Update()
    {
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            Countdown = 100;
            if (waveToSpawn == -1)
            {
                StartCoroutine(SpawnRandom(20));
                Debug.Log("Random Wave");
            }
            else if(waveToSpawn >= 0)
            {
                StartCoroutine(Spawn(waves[waveToSpawn]));
                Debug.Log("Formation Wave");
            }
            else
            {
                Debug.Log("No Wave");
            }
        }
    }

    //Spawn formation wave number "wavetoSpawn"
    IEnumerator Spawn(Wave wavetoSpawn)
    {
        List<ZombieSpawn> wave = new List<ZombieSpawn>(wavetoSpawn.zombies);
        float waveCounter = 0;
        while (wave.Count > 0)
        {
            for (int i = 0; i < wave.Count; i++)
            {
                if (wave[i].activationDelay < waveCounter)
                {
                    if (wave[i].spawnPoint != null)
                    {
                        Zombie temp = Instantiate(wave[i].zombie, wave[i].spawnPoint.position, new Quaternion(0, 0, 0, 0));
                        temp.transform.LookAt(wave[i].waypoint.position);
                        temp.SetWaypoint(wave[i].waypoint.position);
                    }
                    wave.RemoveAt(i);
                        i--;
                }
            }
            waveCounter += Time.deltaTime;
            yield return null;
        }
    }

    //Spawn "num" zombies randomly from different doors
    IEnumerator SpawnRandom(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int randomSpawn = Random.Range(0, spawnPoints.Length);
            Zombie temp = Instantiate(prefab, spawnPoints[randomSpawn]);
            switch (randomSpawn)
            {
                case 0: temp.SetWaypoint(randomBackLeftSpawnPoints[Random.Range(0, randomBackLeftSpawnPoints.Length)].position); break;
                case 1: temp.SetWaypoint(randomBackRightSpawnPoints[Random.Range(0, randomBackRightSpawnPoints.Length)].position); break;
                case 2: temp.SetWaypoint(randomMidLeftSpawnPoints[Random.Range(0, randomMidLeftSpawnPoints.Length)].position); break;
                case 3: temp.SetWaypoint(randomMidRightSpawnPoints[Random.Range(0, randomMidRightSpawnPoints.Length)].position); break;
                case 4: temp.SetWaypoint(randomFrontLeftSpawnPoints[Random.Range(0, randomFrontLeftSpawnPoints.Length)].position); break;
                case 5: temp.SetWaypoint(randomFrontRightSpawnPoints[Random.Range(0, randomFrontRightSpawnPoints.Length)].position); break;
                default: break;
            }
            yield return new WaitForSeconds(randomGapTime);
        }
    }
}

[System.Serializable]
public class Wave
{
    public string name;
    public List<ZombieSpawn> zombies;
    //public float timeToNextZombie;
    //public float timeToNextWave;
}

[System.Serializable]
public class ZombieSpawn
{
    public Zombie zombie;
    public Transform spawnPoint;
    public Transform waypoint;
    public float activationDelay;
}
