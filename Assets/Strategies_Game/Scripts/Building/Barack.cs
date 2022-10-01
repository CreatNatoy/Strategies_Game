using UnityEngine;

public class Barack : Building
{
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private Transform _spawnTransform;

    private ServiceLocator _serviceLocator;
    private CreatorUnit _creatorUnit;

    public override void Awake() {
        base.Awake();
        _serviceLocator = ServiceLocator.Instance;
    }

    public override void Start() {
        base.Start();
        _creatorUnit = _serviceLocator.Get<CreatorUnit>();
    }

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
        var newUnit = _creatorUnit.GetKnight();
        newUnit.ResetHealth();
        newUnit.gameObject.transform.position = spawnPosition;
       var position = spawnPosition +
                          new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
       newUnit.WhenClickOnGround(position);
    }
    
}
