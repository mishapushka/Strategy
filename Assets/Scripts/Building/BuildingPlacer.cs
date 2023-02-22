using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{

    public float CellSize = 1f;
    public Camera RaycastCamera;

    private Plane _plane;
    public Building CurrentBuilding;

    public Dictionary<Vector2Int, Building> BuildingDictionary = new Dictionary<Vector2Int, Building>();

    private void Start() {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update() {
        if (CurrentBuilding == null) {
            return;
        }

        Ray ray = RaycastCamera.ScreenPointToRay(Input.mousePosition);

        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;

        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);

        CurrentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;

        // красный дом или нет когда ставишь друг на друга
        if (CheckAllow(x, z, CurrentBuilding)) {
            CurrentBuilding.DisplayAcceptablePosition();
            if (Input.GetMouseButtonDown(0)) {
                InstallBuilding(x, z, CurrentBuilding);
                CurrentBuilding = null;
            }
        } else {
            CurrentBuilding.DisplayUnacceptablePosition();
        }
    }

    bool CheckAllow(int xPosition, int zPosition, Building building) {
        for (int x = 0; x < building.XSize; x++) {
            for (int z = 0; z < building.ZSize; z++) {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (BuildingDictionary.ContainsKey(coordinate)) {
                    return false;
                }
            }
        }
        return true;
    }

    void InstallBuilding(int xPosition, int zPosition, Building building) {
        for (int x = 0; x < building.XSize; x++) {
            for (int z = 0; z < building.ZSize; z++) {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                BuildingDictionary.Add(coordinate, CurrentBuilding);
            }
        }
        // выводит в консоль ошибку когда строишь здание поверх другого
        //foreach (var item in BuildingDictionary) {
        //    Debug.Log(item);
        //}
    }

    public void CreateBuilding(GameObject buildingPrefab) {
        GameObject newBuilding = Instantiate(buildingPrefab);
        CurrentBuilding = newBuilding.GetComponent<Building>();
    }
}
