using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private float _cellSize = 1f;
    
    private Plane _plane;
   [SerializeField] private Building _currentBuilding;
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
        if(_currentBuilding == null) return; 
        
        _currentBuilding.transform.position = GetPointRaycast();

        if (Input.GetMouseButtonDown(0)) {
            _currentBuilding = null; 
        }
    }

    private Vector3 GetPointRaycast() {
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);

        _plane.Raycast(ray, out var distance);
        var point = ray.GetPoint(distance) / _cellSize;
        
        var x = Mathf.RoundToInt(point.x);
        var z = Mathf.RoundToInt(point.z); 
        
        return new Vector3(x,0,z) * _cellSize;
    }
    
    private void OnValidate() {
        CellSize = _cellSize;
    }

    public void CreateBuilding(Building buildingPrefab) {
       var newBuilding = Instantiate(buildingPrefab);
       _currentBuilding = newBuilding;
    }
}
