using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BulletLoadingUI : MonoBehaviour
{
    private float bulletLoadTime;
    public Image loadImage;
    private float timeSince;
    private float timeStamp;

    private void Start()
    {
        bulletLoadTime = MainPlayerMovement.MainPlayer.bulletAutoRefillTime;
        timeSince = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        timeStamp = Time.timeSinceLevelLoad - timeSince;

        if(!MainPlayerMovement.MainPlayer.isBulletFull)
        {
            loadImage.fillAmount = timeStamp / bulletLoadTime;
            if (timeStamp > bulletLoadTime)
            {
                timeSince = Time.timeSinceLevelLoad;
            }
        }
    }
}
