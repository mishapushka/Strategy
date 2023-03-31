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

    [SerializeField] protected Animator _animator;

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
            Die();
        } else {
            _animator.SetTrigger("Hit");
        }
    }

    void Die() {
        // +effect
        Destroy(this);
        Destroy(NavMeshAgent);
        _animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }

    private void OnDestroy() {
        FindObjectOfType<Management>().Unselect(this);
        if (_healthBar) {
            Destroy(_healthBar.gameObject);
        }
    }
}
