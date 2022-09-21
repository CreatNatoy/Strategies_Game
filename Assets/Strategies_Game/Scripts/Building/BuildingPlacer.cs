using System;
using UnityEngine;

[ExecuteAlways]
public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private float _cellSize = 1f; 
    public static float CellSize = 1f;

    private void Awake() {
        CellSize = _cellSize; 
    }

    private void OnValidate() {
        CellSize = _cellSize;
    }
}
