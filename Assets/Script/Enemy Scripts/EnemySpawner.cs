using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int numberOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}


public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [SerializeField] private Wave[] waves;
    [SerializeField] private float spawnRadius;
    [SerializeField] private GameObject spawnAnimation;
    [SerializeField] private float nextWavePauseTime;
    [SerializeField] private int totalNumberOfWaves;

    private List<GameObject> enemyPool;

    private Wave currentWave;
    private int currentWaveNumber = 0;

    private bool canSpawn = true;
    private int enemyCount = 0;

    private float nextSpawnTime;

    private GameObject randomEnemy;

    private void Start()
    {
        instance = this;

        if (enemyPool == null)
            enemyPool = new List<GameObject>();

        currentWave = waves[currentWaveNumber];
    }


    private void Update()
    {

        if(enemyCount <= currentWave.numberOfEnemies && nextSpawnTime < Time.time)
        {
            currentWave = waves[currentWaveNumber];
            StartCoroutine(Spawner());
            
            nextSpawnTime = Time.time + currentWave.spawnInterval;

        }
    }

    IEnumerator Spawner()
    {
        randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length - 1)];
        
        Vector2 playerPos =  MainPlayerMovement.MainPlayer.transform.position;

        Vector2 spawnPoints = Random.insideUnitCircle*spawnRadius + playerPos ;
        GameObject animation = Instantiate(spawnAnimation, spawnPoints, Quaternion.identity);

        yield return StartCoroutine(SpawnEnemy(spawnPoints));
    }

    IEnumerator SpawnEnemy(Vector2 spawnOn)
    {

        yield return new WaitForSeconds(2.2f);

        GameObject spawnedEnemy = Instantiate(randomEnemy, spawnOn, Quaternion.identity);
        enemyPool.Add(spawnedEnemy);
        enemyCount++;
    }

    public void UpdateEnemyPool(GameObject enemyToRemove)
    {
        enemyCount--;
        enemyPool.Remove(enemyToRemove);

        if (enemyCount <= currentWave.numberOfEnemies )
        {
            currentWave = waves[currentWaveNumber];
            StartCoroutine(Spawner());
        }

    }

    public void EnemyKilled(GameObject enemy)
    {
        if (enemy != null)
        {
            enemyPool.Remove(enemy);

            if (enemyPool.Count == 0)
            {
                currentWaveNumber++;
                enemyCount = 0;
                nextSpawnTime = Time.time + nextWavePauseTime;

                if (currentWaveNumber == totalNumberOfWaves -1)
                {
                    canSpawn = false;
                }
                else
                    canSpawn = true;


            }


        }
    }
}
