using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Currency;
    public int startCurrency = 300;

    public static int Lives;
    public int startLives = 20;

    public static int WavesSurvived;

    private void Start()
    {
        Currency = startCurrency;
        Lives = startLives;

        WavesSurvived = 0;
    }
}
