using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject {

    public NavMeshAgent NavMeshAgent;
    public int Price;
    public int Health;

    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        NavMeshAgent.SetDestination(point);
    }

    public void TakeDamage(int damageValue) {
        Health -= damageValue;
        if (Health <= 0) {
            // Die
            Destroy(gameObject);
        }
    }
}
