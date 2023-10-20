using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;
    public Button[] buttons;
    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < buttons.Length; i++)
        {
            if(i + 1 > levelReached)
            {
                buttons[i].interactable = false;
            }
        }
    }
    public void Select (string levelName)
    {
        fader.FadeTo(levelName);
    }
}
