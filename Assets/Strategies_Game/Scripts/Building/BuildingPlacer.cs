using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private float _cellSize = 1f;

    private Plane _plane;
    private Dictionary<Vector2Int, Building> _buildingsDictionary = new Dictionary<Vector2Int, Building>(); 
    private Building _currentBuilding;
    private ServiceLocator _serviceLocator;

    public static float CellSize = 1f;

    private void Awake() {
        CellSize = _cellSize;

        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.Register(this);
    }

    private void Start() {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update() {
        if (_currentBuilding == null) return;

        _currentBuilding.transform.position = GetPointRaycast(out var x, out var z);

        if (CheckAllow(x, z)) {
            _currentBuilding.DisplayAcceptablePosition();
            
            if (Input.GetMouseButtonDown(0)) {
                InstallBuilding(x,z);
                _currentBuilding = null;
            }
        }
        else {
            _currentBuilding.DisplayUnacceptablePosition();
        }
        
    }

    private bool CheckAllow(int xPosition, int zPosition) {
        for (var x = 0; x < _currentBuilding.XSize; x++) {
            for (var z = 0; z < _currentBuilding.ZSize; z++) {
                var coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (_buildingsDictionary.ContainsKey(coordinate)) return false; 
            }
        }
        return true;
    }

    private void InstallBuilding(int xPosition, int zPosition) {
        for (var x = 0; x < _currentBuilding.XSize; x++) {
            for (var z = 0; z < _currentBuilding.ZSize; z++) {
                var coordinate = new Vector2Int(xPosition + x, zPosition + z); 
                _buildingsDictionary.Add(coordinate, _currentBuilding);
            }
        }
    }

    private Vector3 GetPointRaycast(out int x, out int z) {
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);

        _plane.Raycast(ray, out var distance);
        var point = ray.GetPoint(distance) / _cellSize;

        x = Mathf.RoundToInt(point.x);
        z = Mathf.RoundToInt(point.z);

        return new Vector3(x, 0, z) * _cellSize;
    }

    private void OnValidate() {
        CellSize = _cellSize;
    }

    public void CreateBuilding(Building buildingPrefab) {
        var newBuilding = Instantiate(buildingPrefab);
        _currentBuilding = newBuilding;
    }
}