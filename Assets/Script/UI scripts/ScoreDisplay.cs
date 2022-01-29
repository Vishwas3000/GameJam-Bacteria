using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public static ScoreDisplay instance;

    private Text scoreText;
    private bool needUpdate = false;
    private Animator animator;
    [SerializeField] private float timeLimit;
    private float timeStamp;

    void Awake()
    {
        instance = this;

        scoreText = GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    public void UpdateScore(int currScore)
    {
        scoreText.text = currScore.ToString();
        needUpdate = true;
        animator.enabled = true;
        timeStamp = Time.time;

    }

    private void Update()
    {
        timeStamp = Time.time + timeLimit;

        if(needUpdate)
        {
            Debug.Log("timeStamp: " + timeStamp + " & time.time: " + Time.time);

            if(timeStamp < Time.time)
            {
                needUpdate = false;
                animator.enabled = false;
            }
        }
    }
}
