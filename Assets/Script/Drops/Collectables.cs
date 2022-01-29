using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private int healPoint = 5,
                bulletRefilPoint = 3,
                DashPoint = 3;

    public float dissapearTime = 6f;

    private Color iniColor;
    private Color finalColor;
    private float timeSince;
    float timeSinceEnstanciated;

    private static int count = 0;

    public void Start()
    {
        finalColor = iniColor = GetComponent<SpriteRenderer>().color;
        finalColor.a = 0f;

        timeSince = Time.timeSinceLevelLoad;
        count++;
    }

    private void Update()
    {
        timeSinceEnstanciated = Time.timeSinceLevelLoad - timeSince;
        if(timeSinceEnstanciated > dissapearTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "MainPlayer")
        {
            MainPlayerMovement.MainPlayer.UpdateHealth(healPoint);
            MainPlayerMovement.MainPlayer.UpdateBullet(bulletRefilPoint);
            //MainPlayerMovement.MainPlayer.

            AudioManager.instance.Play("PlayerCollected");

            Destroy(gameObject);

        }
    }
}
