using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{

    public Transform Spawn;
    public float CreationPeriod;
    public GameObject EnemyPrefab;

    private float _timer;

    private void Update() {
        _timer += Time.deltaTime;
        if (_timer > CreationPeriod) {
            _timer = 0f;
            Instantiate(EnemyPrefab, Spawn.position, Spawn.rotation);
        }
    }
}
