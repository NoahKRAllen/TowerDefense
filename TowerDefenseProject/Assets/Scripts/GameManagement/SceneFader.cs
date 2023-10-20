using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneFader : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private AnimationCurve fadeCurve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 1f;
        while(time > 0)
        {
            time -= Time.deltaTime;
            float alpha = fadeCurve.Evaluate(time);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return 0;
        }
    }
    
    public void FadeTo(string sceneToLoad)
    {
        StartCoroutine(FadeOut(sceneToLoad));
    }

    IEnumerator FadeOut(string scene)
    {
        float time = 0f;
        while(time < 1)
        {
            time += Time.deltaTime;
            float alpha = fadeCurve.Evaluate(time);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return 0;
        }
        SceneManager.LoadScene(scene);
    }

    
}
