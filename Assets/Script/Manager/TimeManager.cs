using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public float slowdownFactor = 0.05f;
    public float slowdownLenght = 2f;

    private bool isOperationNeeded = false;


    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        if(isOperationNeeded)
        {

            Time.timeScale += (1f / slowdownLenght) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            if(Time.timeScale == 1f)
            { 
                Time.fixedDeltaTime = 0.02f;
                isOperationNeeded = false;
            }
        }

    }

    public void DoSlowMotion()
    {      
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime *= Time.timeScale ;
        isOperationNeeded = true;
    }
}
