using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgress : MonoBehaviour
{
    public SceneFader fader;
    public string mainMenuName;
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        fader.FadeTo(mainMenuName);
    }
    
    
}
