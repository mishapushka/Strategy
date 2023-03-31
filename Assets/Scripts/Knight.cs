using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState {
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}

public class Knight : Unit
{

    public UnitState CurrentUnitState;

    public Enemy TargetEnemy;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;
    public Vector3 TargetPoint;

    public float AttackPeriod = 1f;
    private float _timer;

    public override void Start() {
        base.Start();
        SetState(UnitState.WalkToPoint);
    }

    private void Update() {
        if (CurrentUnitState == UnitState.Idle) {
            FindClosestEnemy();
        } else if (CurrentUnitState == UnitState.WalkToPoint) {
            FindClosestEnemy();
            if (Vector3.Distance(transform.position, TargetPoint) < 0.5f) {
                SetState(UnitState.Idle);
            }

        } else if (CurrentUnitState == UnitState.WalkToEnemy) {

            if (TargetEnemy) {
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);
                // если юнит от врага ушел далеко, то отменил следование до него
                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distance > DistanceToFollow) {
                    SetState(UnitState.WalkToPoint);
                }
                // когда враг подошел близко к юниту, он начал его атаковать
                if (distance < DistanceToAttack) {
                    SetState(UnitState.Attack);
                }
            } else {
                SetState(UnitState.WalkToPoint);
            }

        } else if (CurrentUnitState == UnitState.Attack) {

            if (TargetEnemy) {
                NavMeshAgent.SetDestination(TargetEnemy.transform.position);

                // перестает атаковать и за ним идет
                float distance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
                if (distance > DistanceToAttack) {
                    SetState(UnitState.WalkToEnemy);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod) {
                    _timer = 0;
                    // нанести урон
                    TargetEnemy.TakeDamage(1);
                }
            } else {
                SetState(UnitState.WalkToPoint);
            }
        }
    }

    public void SetState(UnitState unitState) {
        CurrentUnitState = unitState;
        switch (CurrentUnitState) {
            case UnitState.Idle:
                _animator.SetTrigger("Idle");
                NavMeshAgent.stoppingDistance = 0.2f;
                break;
            case UnitState.WalkToPoint:
                NavMeshAgent.SetDestination(TargetPoint);
                _animator.SetTrigger("Walking");
                NavMeshAgent.stoppingDistance = 0.05f;
                break;
            case UnitState.WalkToEnemy:
                _animator.SetTrigger("Walking");
                NavMeshAgent.stoppingDistance = 1f;
                break;
            case UnitState.Attack:
                _animator.SetTrigger("Attack");
                NavMeshAgent.stoppingDistance = 1f;
                _timer = 0;
                break;
            default:
                break;
        }
        //if (CurrentUnitState == UnitState.Idle) {
        //    NavMeshAgent.stoppingDistance = 0.2f;
        //} else if (CurrentUnitState == UnitState.WalkToPoint) {
        //    NavMeshAgent.stoppingDistance = 0.05f;
        //} else if (CurrentUnitState == UnitState.WalkToEnemy) {
        //    NavMeshAgent.stoppingDistance = 1f;
        //} else if (CurrentUnitState == UnitState.Attack) {
        //    NavMeshAgent.stoppingDistance = 1f;
        //    _timer = 0;
        //}
    }

    public void FindClosestEnemy() {

        Enemy[] alEnemies = FindObjectsOfType<Enemy>();

        float minDistance = Mathf.Infinity;
        Enemy closesEnemy = null;

        for (int i = 0; i < alEnemies.Length; i++) {
            float distance = Vector3.Distance(transform.position, alEnemies[i].transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closesEnemy = alEnemies[i];
            }
        }
        if (minDistance < DistanceToFollow) {
            TargetEnemy = closesEnemy;
            SetState(UnitState.WalkToEnemy);
        }
    }

    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);
        TargetPoint = point;
        SetState(UnitState.WalkToPoint);
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
}
