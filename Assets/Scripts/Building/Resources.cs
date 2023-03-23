using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{

    public int Money = 100;
    public Text MoneyText;

    public void UpdateMoneyText() {
        MoneyText.text = Money.ToString();
    }
}
