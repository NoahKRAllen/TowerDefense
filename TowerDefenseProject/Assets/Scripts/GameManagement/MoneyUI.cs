using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;
    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + PlayerStats.Currency.ToString();
    }
}
