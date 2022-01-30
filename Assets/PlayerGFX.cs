using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGFX : MonoBehaviour
{
    public static PlayerGFX instance;

    public GameObject onPlayerDestroyedEffect;

    private void Start()
    {
        instance = this;
    }

    public void SpawnEffect()
    {
        Instantiate(onPlayerDestroyedEffect, MainPlayerMovement.MainPlayer.transform.position, MainPlayerMovement.MainPlayer.transform.rotation);
        gameObject.SetActive(false) ;
    }
    
}
