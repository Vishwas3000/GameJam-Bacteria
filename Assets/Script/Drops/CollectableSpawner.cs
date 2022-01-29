using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Drops
{
    public string DropName;
    public GameObject DropItem;
    public float DropFrequency;
}


public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private Drops[] drops;

    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnTime;

    


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
