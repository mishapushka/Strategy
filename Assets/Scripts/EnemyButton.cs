using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyButton : MonoBehaviour
{

    public GameObject UnitPrefab;
    public Text PriceText;
    public BarackRobots BarackRobots;

    private void Start() {
        PriceText.text = UnitPrefab.GetComponent<Unit>().Price.ToString();
    }

    public void TryBy() {
        int price = UnitPrefab.GetComponent<Unit>().Price;

        if (FindObjectOfType<Resources>().Money >= price) {

            FindObjectOfType<Resources>().Money -= price;

            // создать юнита
            BarackRobots.CreateUnit(UnitPrefab);
        } else {
            Debug.Log("Мало золота");
        }
    }
}
