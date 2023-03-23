using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{

    public BuildingPlacer BuildingPlacer;
    public GameObject BuildingPrefab;
    public Text PriceText;

    private void Start() {
        PriceText.text = BuildingPrefab.GetComponent<Building>().Price.ToString();
    }

    public void TryBy() {
        int price = BuildingPrefab.GetComponent<Building>().Price;

        if (FindObjectOfType<Resources>().Money >= price) {
            FindObjectOfType<Resources>().Money -= price;
            BuildingPlacer.CreateBuilding(BuildingPrefab);
        } else {
            Debug.Log("Мало золота");
        }
    }
}
