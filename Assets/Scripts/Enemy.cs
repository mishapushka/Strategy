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
    public Building TargetBuilding;
    public Unit TargetUnit;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;

    public NavMeshAgent NavMeshAgent;

    private void Update() {
        if (CurrentEnemyState == EnemyState.Idle) {

        } else if (CurrentEnemyState == EnemyState.WalkToBuilding) {

        } else if (CurrentEnemyState == EnemyState.WalkToUnit) {

        } else if (CurrentEnemyState == EnemyState.Attack) {

        }
    }

    public void SetState(EnemyState enemyState) {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle) {

        } else if (CurrentEnemyState == EnemyState.WalkToBuilding) {

        } else if (CurrentEnemyState == EnemyState.WalkToUnit) {

        } else if (CurrentEnemyState == EnemyState.Attack) {

        }
    }
}
