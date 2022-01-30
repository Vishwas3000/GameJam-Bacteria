using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public bool playerDead = false;

    public float delayTime = 2.0f;


    private void Awake()
    {
        gameManager = this;
    }

    private void Start()
    {
        if(EnemyAI.EnemyRbs != null && EnemyAI.EnemyRbs.Count > 0 )
        {
            EnemyAI.EnemyRbs.Clear();

        }
        
    }
    public void EndGame()
    {
        if(playerDead == false)
        {
            playerDead = true;
        }


        if(playerDead==true)
        {
            PlayerDead();
        }

    }

    public void PlayerDead()
    {
        if(GameOverScreen.instance==null)
        {
            Debug.Log("gameOverScreen is null");
        }

        GameOverScreen.instance.Setup();

    }

    public void Restart()
    {
        SceneManager.LoadScene("Level1");
        EnemyAI.EnemyRbs.Clear();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
