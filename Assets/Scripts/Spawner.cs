using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float Countdown;

    //Preset formation waves
    [SerializeField] private Formation[] formations;
    [SerializeField] private int waveToSpawn = 0;


    [SerializeField] private Waves[] waves;
    private int waveIndex = 0;

    [Header("Random Wave Waypoints")]
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
            if (waves.Length >= waveIndex + 1)
            {
                Countdown = waves[waveIndex].timeToNextWave;
                waveToSpawn = waves[waveIndex].waveToSpawn;
                if (waveToSpawn == -1)
                {
                    StartCoroutine(SpawnRandom(waves[waveIndex].numberToSpawn));
                    Debug.Log("Random Wave");
                }
                else if (waveToSpawn >= 0)
                {
                    StartCoroutine(Spawn(formations[waveToSpawn]));
                    Debug.Log("Formation Wave");
                }
                else
                {
                    Debug.Log("No Wave");
                }
                waveIndex++;
            }
            else
            {
                waveIndex = 0;
            }
        }
    }

    //Spawn formation wave number "wavetoSpawn"
    IEnumerator Spawn(Formation wavetoSpawn)
    {
        Zombie prefab = waves[waveIndex].zombie;
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
                        Zombie temp = Instantiate(prefab, wave[i].spawnPoint.position, new Quaternion(0, 0, 0, 0));
                        temp.transform.LookAt(wave[i].waypoint.position);
                        temp.SetWaypoint(wave[i].waypoint.gameObject);
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
        float waitTime = waves[waveIndex].spawnGapTime;
        Zombie prefab = waves[waveIndex].zombie;
        for (int i = 0; i < num; i++)
        {
            int randomSpawn = Random.Range(0, spawnPoints.Length);
            Zombie temp = Instantiate(prefab, spawnPoints[randomSpawn].position, new Quaternion(0,0,0,0));
            switch (randomSpawn)
            {
                case 0: temp.SetWaypoint(randomBackLeftSpawnPoints[Random.Range(0, randomBackLeftSpawnPoints.Length)].gameObject); break;
                case 1: temp.SetWaypoint(randomBackRightSpawnPoints[Random.Range(0, randomBackRightSpawnPoints.Length)].gameObject); break;
                case 2: temp.SetWaypoint(randomMidLeftSpawnPoints[Random.Range(0, randomMidLeftSpawnPoints.Length)].gameObject); break;
                case 3: temp.SetWaypoint(randomMidRightSpawnPoints[Random.Range(0, randomMidRightSpawnPoints.Length)].gameObject); break;
                case 4: temp.SetWaypoint(randomFrontLeftSpawnPoints[Random.Range(0, randomFrontLeftSpawnPoints.Length)].gameObject); break;
                case 5: temp.SetWaypoint(randomFrontRightSpawnPoints[Random.Range(0, randomFrontRightSpawnPoints.Length)].gameObject); break;
                default: break;
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}

[System.Serializable]
public class Formation
{
    public string name;
    public List<ZombieSpawn> zombies;
    //public float timeToNextZombie;
    //public float timeToNextWave;
}

[System.Serializable]
public class ZombieSpawn
{
    public Transform spawnPoint;
    public Transform waypoint;
    public float activationDelay;
}

[System.Serializable]
public class Waves
{
    public Zombie zombie;
    public int waveToSpawn;
    public int numberToSpawn = 0;
    public float spawnGapTime = 0;
    public float timeToNextWave;
}