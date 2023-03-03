using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barack : Building
{

    public Transform Spawn;

    public void CreateUnit(GameObject unitPrefab) {
        GameObject newUnit = Instantiate(unitPrefab, Spawn.position, Quaternion.identity);
        Vector3 position = Spawn.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        newUnit.GetComponent<Unit>().WhenClickOnGround(position);
    }
}
