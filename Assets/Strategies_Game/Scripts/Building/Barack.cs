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
        var spawnPosition = _spawnTransform.position;
        var newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
       var position = spawnPosition +
                          new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
       newUnit.WhenClickOnGround(position);
    }
    
}
