using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState {
    UnitsSelected,
    Frame,
    Other
}

public class Management : MonoBehaviour {

    public Camera Camera;
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    public Image FrameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    public SelectionState CurrentSelectionState;

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
                CurrentSelectionState = SelectionState.UnitsSelected;
                Select(Hovered);
            }
        }

        // здесь можно делать хоть что, когда юниты выделены
        if (CurrentSelectionState == SelectionState.UnitsSelected) {

            if (Input.GetMouseButtonUp(0)) {
                if (hit.collider.tag == "Ground") {
                    for (int i = 0; i < ListOfSelected.Count; i++) {
                        ListOfSelected[i].WhenClickOnGround(hit.point);
                    }
                }
            }
        }


        if (Input.GetMouseButtonDown(1)) {
            UnSelectAll();
        }

        // Выделение рамкой
        if (Input.GetMouseButtonDown(0)) {
            _frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);
            Vector2 size = max - min;

            if (size.magnitude > 10) {

                FrameImage.enabled = true;

                FrameImage.rectTransform.anchoredPosition = min;

                FrameImage.rectTransform.sizeDelta = size;

                Rect rect = new Rect(min, size);

                UnSelectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++) {
                    Vector2 screenPosition = Camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPosition)) {
                        Select(allUnits[i]);
                    }
                }

                CurrentSelectionState = SelectionState.Frame;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            FrameImage.enabled = false;
            if (ListOfSelected.Count > 0) {
                CurrentSelectionState = SelectionState.UnitsSelected;
            } else {
                CurrentSelectionState = SelectionState.Other;
            }
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
        CurrentSelectionState = SelectionState.Other;
    }

    void UnHoverCurrent() {
        if (Hovered) {
            Hovered.OnUnHover();
            Hovered = null;
        }
    }
}

