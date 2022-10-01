using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] private int _price = 5;
    [SerializeField] private int _health;
    [SerializeField] private HealthBar _healthBarPrefab;

    private int _maxHealth;
    private ServiceLocator _serviceLocator;
    private Management _management; 
    
    public NavMeshAgent _navMeshAgent;
    public int Price => _price;

    private void Awake() {
        _maxHealth = _health; 
    }

    public override void Start() {
        base.Start();
        _healthBarPrefab = Instantiate(_healthBarPrefab);
        _healthBarPrefab.Setup(transform);
        
        _serviceLocator = ServiceLocator.Instance;
        _management = _serviceLocator.Get<Management>(); 
    }
    
    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        _navMeshAgent.SetDestination(point); 
    }

    public bool TryKilled(int damageValue) {
        _health -= damageValue;
        _healthBarPrefab.SetHealth(_health,_maxHealth);
        if (_health <= 0) {
            Die();
            return true; 
        }

        return false; 
    }

    private void Die() {
        gameObject.SetActive(false);
        _management.Unselect(this);
    }

    public void ResetHealth() {
        _health = _maxHealth;
        _healthBarPrefab.SetHealth(_health, _maxHealth);
    }
}
