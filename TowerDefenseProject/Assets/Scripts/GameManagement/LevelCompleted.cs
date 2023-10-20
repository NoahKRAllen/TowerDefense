using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LevelCompleted : MonoBehaviour
{
    [SerializeField]
    private Text totalPoints;

    private int newPointsTotal;
    private int prevPoints;
    private int tempPoints;
    private void OnEnable()
    {
        tempPoints = PlayerPrefs.GetInt("TempPoints");
        Debug.Log(tempPoints);
        prevPoints = PlayerPrefs.GetInt("Points");
        Debug.Log(prevPoints);
        newPointsTotal = prevPoints + tempPoints;

        StartCoroutine(AnimateValue());
        
        PlayerPrefs.SetInt(("levelReached"), 2);
    }

    IEnumerator AnimateValue()
    {

        totalPoints.text = PlayerPrefs.GetInt("Points", 0).ToString();

        yield return new WaitForSeconds(2.0f);

        while(prevPoints < newPointsTotal)
        {
            prevPoints++;
            totalPoints.text = prevPoints.ToString();
            yield return new WaitForSeconds(.05f);
        }
        PlayerPrefs.SetInt("Points", newPointsTotal);
    }
}
