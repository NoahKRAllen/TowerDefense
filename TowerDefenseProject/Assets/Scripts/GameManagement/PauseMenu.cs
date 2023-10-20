using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;
    [SerializeField]
    private SceneFader sceneFader;
    [SerializeField]
    private Wavespawner spawnerInstance;
    private void Update()
    {
        if(spawnerInstance.gameIsOver)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.P))
        {
            Toggle();
        }
    }
    public void Toggle()
    {
        pauseUI.SetActive(!pauseUI.activeSelf);

        if(pauseUI.activeSelf)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
    public void Retry()
    {
        Time.timeScale = 1.0f;
        sceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        sceneFader.FadeTo("MainMenu");
    }
}
