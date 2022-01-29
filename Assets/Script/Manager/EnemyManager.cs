using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManager;

    private float multiDashTime;
    private float enemyKillTimeSince;
    private float enemyKillStamp;
    public int dashCounter = 0;

    private void Start()
    {
        enemyManager = this;

        enemyKillStamp = Time.timeSinceLevelLoad;
        multiDashTime = MainPlayerMovement.MainPlayer.multiDashTimeLimit;
    }

    public void EnemyGotKiller()
    {
        enemyKillTimeSince = Time.timeSinceLevelLoad - enemyKillStamp;

        enemyKillStamp = Time.timeSinceLevelLoad;

        if (enemyKillTimeSince <= multiDashTime)
        {
            MainPlayerMovement.MainPlayer.multiDashing = true;
            dashCounter++;
            ScoreMultiplerOnDash.instance.TextUpdate(dashCounter);
        }
        else
        {
            MainPlayerMovement.MainPlayer.multiDashing = false;
            dashCounter = 0;
        }

    }


    private void Update()
    {
        enemyKillTimeSince = Time.timeSinceLevelLoad - enemyKillStamp;

        if (MainPlayerMovement.MainPlayer.multiDashing ==true )
        {
            if (enemyKillTimeSince > multiDashTime)
                MainPlayerMovement.MainPlayer.multiDashing = false;

        }
    }

}
