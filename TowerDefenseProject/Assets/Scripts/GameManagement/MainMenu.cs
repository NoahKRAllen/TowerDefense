using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string LevelToLoad;

    [SerializeField]
    private SceneFader sceneFader;
    public void Play()
    {
        sceneFader.FadeTo(LevelToLoad);
    }
    public void Quit()
    {
        Application.Quit();
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("Points", 0) == 0)
        {
            PlayerPrefs.SetInt("Points", 0);
        }
    }
}
