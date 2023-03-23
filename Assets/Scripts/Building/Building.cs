using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : SelectableObject
{

    public int Price;
    public int XSize = 3;
    public int ZSize = 3;

    private Color _startColor;
    public Renderer Renderer;

    public GameObject MenuObject;

    public override void Start() {
        base.Start();
        MenuObject.SetActive(false);
    }

    private void Awake() {
        _startColor = Renderer.material.color;
    }

    private void OnDrawGizmos() {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;

        for (int x = 0; x < XSize; x++) {
            for (int z = 0; z < ZSize; z++) {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1f, 0f, 1f) * cellSize);
            }
        }
    }

    public override void Select() {
        base.Select();
        MenuObject.SetActive(true);
    }

    public override void UnSelect() {
        base.UnSelect();
        MenuObject.SetActive(false);
    }

    public void DisplayUnacceptablePosition() {
        Renderer.material.color = Color.red;
    }

    public void DisplayAcceptablePosition() {
        Renderer.material.color = _startColor;
    }
}
