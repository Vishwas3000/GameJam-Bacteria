using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType
{
    MainPlayer,
    Enemy
}


public class Shooter : MonoBehaviour
{
    [SerializeField]
    private Transform firePoint;

    [SerializeField] ObjectType type;

    [SerializeField]
    private GameObject bulletPrefab;

    private int curretBullet;
    private float timeStamp;
    private float timeSince = 0;
    private EnemyAI enemy;

    [SerializeField] private float ShootTime = 5f;

    private void Start()
    {

        switch (type)
        {
            case ObjectType.MainPlayer:

                curretBullet = MainPlayerMovement.MainPlayer.currentBullet;
                
                break;

            case ObjectType.Enemy:

                enemy = gameObject.GetComponent<EnemyAI>();

                
                break;

            default:

                break;

        }


    }

    private void Update()
    {
        switch(type)
        {
            case ObjectType.MainPlayer:

                curretBullet = MainPlayerMovement.MainPlayer.currentBullet;

                if(curretBullet>0)
                {
                    if (Input.GetButtonDown("Fire1"))
                    {
                        Shoot();
                        MainPlayerMovement.MainPlayer.UpdateBullet(-1);
                        AudioManager.instance.Play("PlayerShooting");
                    }

                }


                break;

            case ObjectType.Enemy:

                if(enemy.slide)
                { 
                    if(timeSince <= Time.time || timeSince ==0)
                    {
                        Shoot();
                        timeSince = Time.time + ShootTime;
                        AudioManager.instance.Play("EnemyShooting");
                    }
                }


                break;

            default:

                break;

        }

    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
