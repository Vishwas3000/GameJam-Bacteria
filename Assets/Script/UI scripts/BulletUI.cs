using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    public static BulletUI instance;
    public Text bulletInfo;

    private int currentBullet;
    private int bulletCapacity;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentBullet = MainPlayerMovement.MainPlayer.currentBullet;
        bulletCapacity = MainPlayerMovement.MainPlayer.bulletCapacity;

        bulletInfo.text = currentBullet.ToString() + "/" + bulletCapacity.ToString();
    }

    public void UpdateBullet(int current, int max)
    {
        bulletInfo.text = current.ToString() + "/" + max.ToString();
    }

    
}
