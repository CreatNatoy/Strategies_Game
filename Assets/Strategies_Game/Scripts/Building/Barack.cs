using UnityEngine;

public class Barack : Building
{
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private Transform _spawnTransform;

    public override void Select() {
        base.Select();
        _menuObject.SetActive(true);
    }
    
    public override void Unselect() {
        base.Unselect();
        _menuObject.SetActive(false);
    }

    public void CreateUnit(Unit unitPrefab) {
        Instantiate(unitPrefab, _spawnTransform.position, Quaternion.identity); 
    }
    
}
