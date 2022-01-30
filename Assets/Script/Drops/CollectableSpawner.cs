using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Drops
{
    public string DropName;
    public GameObject DropItem;
}


public class CollectableSpawner : MonoBehaviour
{
    public static CollectableSpawner instance;

    [SerializeField] private Drops[] drops;

    [SerializeField] private int totalItems;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnTime;


    private float timeStamp;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeStamp = Time.timeSinceLevelLoad+ spawnTime;
        
    }


    void Update()
    {
        if(timeStamp<Time.timeSinceLevelLoad)
        {
            SpawnCollectables();
            timeStamp = Time.timeSinceLevelLoad + spawnTime;
        }
    }

    void SpawnCollectables()
    {
        int randIndex = Random.Range(0, totalItems);

        Vector2 pos = MainPlayerMovement.MainPlayer.transform.position;

        Vector2 spawnPoints = Random.insideUnitCircle * spawnRadius + pos;

        Instantiate(drops[randIndex].DropItem,spawnPoints, Quaternion.identity );

    }
}
