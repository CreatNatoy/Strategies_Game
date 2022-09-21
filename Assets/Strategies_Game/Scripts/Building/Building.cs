using UnityEngine;

public class Building : SelectableObject
{
    [SerializeField] private int _price;
    [SerializeField] private int _xSize = 3;
    [SerializeField] private int _zSize = 3;

    public int Price => _price; 
    
    private void OnDrawGizmos() {
        var cellSize = BuildingPlacer.CellSize; 
        for (var x = 0; x < _xSize; x++) {
            for (var z = 0; z < _zSize; z++) {
                
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * cellSize, new Vector3(1,0,1) * cellSize);
            }
        }
    }
}
