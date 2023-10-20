using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverControl : MonoBehaviour
{
    [SerializeField]
    private Text roundsNumberText;

    private void OnEnable()
    {
        roundsNumberText.text = PlayerStats.WavesSurvived.ToString();
    }
}
