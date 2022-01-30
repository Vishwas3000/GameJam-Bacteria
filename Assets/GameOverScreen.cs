using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen instance;

    public Text finalScore;

    private void Awake()
    {
        instance = this;
    }

    public void Setup()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        finalScore.text = MainPlayerMovement.MainPlayer.playerScore.ToString() + " Points";
    }
}
