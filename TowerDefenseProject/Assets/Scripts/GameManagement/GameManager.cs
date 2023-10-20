using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsOver;

    [SerializeField]
    private SceneFader sceneFader;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject gameWonUI;

    private void Start()
    {
        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameIsOver)
        {
            return;
        }

        if(PlayerStats.Lives <= 0)
        {
            EndLevel();
        }
    }
    private void EndLevel()
    {
        Debug.Log("Game Over!");
        gameOverUI.SetActive(true);
        gameIsOver = true;
    }
    public void WonLevel()
    {
        Debug.Log("You Won");
        //Change to you win UI
        PlayerPrefs.SetInt("levelReached", PlayerPrefs.GetInt("levelReached") + 1);
        gameWonUI.SetActive(true);
        gameIsOver = true;
    }
}
