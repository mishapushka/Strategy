using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour {
    public GameObject SelectionIndicator;

    public virtual void Start() {
        SelectionIndicator.SetActive(false);
    }

    public virtual void OnHover() {
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnUnHover() {
        transform.localScale = Vector3.one;
    }

    public virtual void Select() {
        SelectionIndicator.SetActive(true);
    }

    public virtual void UnSelect() {
        SelectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point) {

    }
}
