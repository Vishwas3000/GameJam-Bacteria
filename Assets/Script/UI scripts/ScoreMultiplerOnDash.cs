using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMultiplerOnDash : MonoBehaviour
{
    public float fadeTime;
    private Text scoreMultiplier;
    private int iniTextSize;
    [SerializeField] private int endTextSize;
    private Color iniColor;
    private Color endColor;
    private float timeStamp = 0;

    public static ScoreMultiplerOnDash instance;


    private void Start()
    {
        instance = this;
        
        scoreMultiplier = GetComponent<Text>();

        iniTextSize = scoreMultiplier.fontSize;

        endColor = iniColor = scoreMultiplier.color;
        endColor.a = 1f;

    }

    public void TextUpdate(int count)
    {
        timeStamp = Time.timeSinceLevelLoad;

        if(scoreMultiplier.enabled ==false)
        {
            scoreMultiplier.enabled = true;
        }
        scoreMultiplier.text = count.ToString() + "X";
    }

    private void Update()
    {
        

        if (scoreMultiplier.enabled)
        {
            float t = Time.timeSinceLevelLoad - timeStamp;

            scoreMultiplier.color = Color.Lerp(iniColor, endColor, t / fadeTime);
            scoreMultiplier.fontSize = (int)Mathf.Lerp(iniTextSize, endTextSize, t / fadeTime);

            if (t>fadeTime)
            {
                scoreMultiplier.enabled = false;
                scoreMultiplier.color = iniColor;
                scoreMultiplier.fontSize = iniTextSize;
            }      
        }
        
    }
}
