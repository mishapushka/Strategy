using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public Transform ScaleTransform;
    public Transform Target;

    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;    
    }


    void LateUpdate()
    {
        transform.position = Target.position + Vector3.up * 4f;
        transform.rotation = _cameraTransform.rotation;
    }

    public void Setup(Transform target) {
        Target = target;
    }

    public void SetHealth(int health, int maxHealth) {
        float xScale = (float)health / maxHealth;
        xScale = Mathf.Clamp01(xScale);
        ScaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
}
