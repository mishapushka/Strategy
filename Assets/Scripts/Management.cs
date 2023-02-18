using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Management : MonoBehaviour {

    public Camera Camera;
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    void Update() {

        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.GetComponent<SelectableColider>()) {
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectableColider>().SelectableObject;
                if (Hovered) {
                    if (Hovered != hitSelectable) {
                        Hovered.OnUnHover();
                        Hovered = hitSelectable;
                        Hovered.OnHover();
                    }
                } else {
                    Hovered = hitSelectable;
                    Hovered.OnHover();
                }
            } else {
                UnHoverCurrent();
            }
        } else {
            UnHoverCurrent();
        }

        if (Input.GetMouseButtonUp(0)) {
            if (Hovered) {
                if (Input.GetKey(KeyCode.LeftControl) == false) {
                    UnSelectAll();
                }
 
                Select(Hovered);
            }

            if (hit.collider.tag == "Ground") {
                for (int i = 0; i < ListOfSelected.Count; i++) {
                    ListOfSelected[i].WhenClickOnGround(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            UnSelectAll();
        }
    }

    void Select(SelectableObject selectableObject) {
        if (ListOfSelected.Contains(selectableObject) == false) {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    void UnSelectAll() {
        for (int i = 0; i < ListOfSelected.Count; i++) {
            ListOfSelected[i].UnSelect();
        }
        ListOfSelected.Clear();
    }

    void UnHoverCurrent() {
        if (Hovered) {
            Hovered.OnUnHover();
            Hovered = null;
        }
    }
}

