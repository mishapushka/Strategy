using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject {

    public NavMeshAgent NavMeshAgent;
    public int Price;
    public int Health;
    private int _maxHealth;

    public GameObject HealthbarPrefab;
    private HealthBar _healthBar;

    public override void Start() {
        base.Start();
        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthbarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        NavMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue) {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0) {
            // Die
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        Destroy(_healthBar.gameObject);
    }
}
