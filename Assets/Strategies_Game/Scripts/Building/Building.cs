using UnityEngine;

public class Building : SelectableObject
{
    [SerializeField] private int _price;
    [SerializeField] private int _xSize = 3;
    [SerializeField] private int _zSize = 3;
    [SerializeField] private Renderer _renderer;
    
    
    private Color _startColor;
    public int XSize => _xSize;
    public int ZSize => _zSize;
    public int Price => _price;

    private void Awake() {
        _startColor = _renderer.material.color;
    }

    private void OnDrawGizmos() {
        var cellSize = BuildingPlacer.CellSize; 
        for (var x = 0; x < _xSize; x++) {
            for (var z = 0; z < _zSize; z++) {
                
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1,0,1) * cellSize);
            }
        }
    }

    public void DisplayUnacceptablePosition() {
        _renderer.material.color = Color.red; 
    }

    public void DisplayAcceptablePosition() {
        _renderer.material.color = _startColor; 
    }
}
