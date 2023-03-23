using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building {

    public float CoinTimePeriod = 3;
    private float _timer;
    public Resources _resources;

    private void Update() {

        _timer += Time.deltaTime;
        if (_timer > CoinTimePeriod) {
            _resources = FindObjectOfType<Resources>();
            _resources.UpdateMoneyText();
            _resources.Money += 1;
            _timer = 0;
        }
    }
}

