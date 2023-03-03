using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour {

    public EnemyState CurrentEnemyState;
    public int Health;
    private int _maxHealth;

    public Building TargetBuilding;
    public Unit TargetUnit;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;

    public NavMeshAgent NavMeshAgent;

    public float AttackPeriod = 1f;
    private float _timer;

    public GameObject HealthbarPrefab;
    private HealthBar _healthBar;

    private void Start() {
        SetState(EnemyState.WalkToBuilding);

        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthbarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    private void Update() {
        if (CurrentEnemyState == EnemyState.Idle) {
            FindClosestUnits();
        } else if (CurrentEnemyState == EnemyState.WalkToBuilding) {
            FindClosestUnits();
            if (TargetBuilding == null) {
                SetState(EnemyState.Idle);
            }

        } else if (CurrentEnemyState == EnemyState.WalkToUnit) {

            if (TargetUnit) {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                // если юнит от врага ушел далеко, то отменил следование до него
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToFollow) {
                    SetState(EnemyState.WalkToBuilding);
                }
                // когда враг подошел близко к юниту, он начал его атаковать
                if (distance < DistanceToAttack) {
                    SetState(EnemyState.Attack);
                }
            } else {
                SetState(EnemyState.WalkToBuilding);
            }

        } else if (CurrentEnemyState == EnemyState.Attack) {

            if (TargetUnit) {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);

                // перестает атаковать и за ним идет
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToAttack) {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod) {
                    _timer = 0;
                    // нанести урон
                    TargetUnit.TakeDamage(1);
                }
            } else {
                SetState(EnemyState.WalkToBuilding);
            }
        }
    }

    public void SetState(EnemyState enemyState) {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle) {

        } else if (CurrentEnemyState == EnemyState.WalkToBuilding) {
            FindClosestBuilding();
            NavMeshAgent.SetDestination(TargetBuilding.transform.position);
        } else if (CurrentEnemyState == EnemyState.WalkToUnit) {

        } else if (CurrentEnemyState == EnemyState.Attack) {
            _timer = 0;
        }
    }

    // поиск нужного здания
    public void FindClosestBuilding() {

        Building[] allBuildings = FindObjectsOfType<Building>();

        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;

        for (int i = 0; i < allBuildings.Length; i++) {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestBuilding = allBuildings[i];
            }
        }

        TargetBuilding = closestBuilding;
    }

    public void FindClosestUnits() {

        Unit[] allUnits = FindObjectsOfType<Unit>();

        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++) {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }
        if (minDistance < DistanceToFollow) {
            TargetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
    }

#if UNITY_EDITOR
    // рисуем круги атаки
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
#endif

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
