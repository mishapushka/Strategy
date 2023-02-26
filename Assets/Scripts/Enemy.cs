using System.Collections;
using System.Collections.Generic;
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
    public Building TargetBuilding;
    public Unit TargetUnit;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;

    public NavMeshAgent NavMeshAgent;

    public float AttackPeriod = 1f;
    private float _timer;

    private void Start() {
        SetState(EnemyState.WalkToBuilding);
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
}
