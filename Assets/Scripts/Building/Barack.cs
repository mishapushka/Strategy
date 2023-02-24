using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barack : Building
{

    public Transform Spawn;

    public void CreateUnit(GameObject unitPrefab) {
        Instantiate(unitPrefab, Spawn.position, Quaternion.identity);
    }
}
