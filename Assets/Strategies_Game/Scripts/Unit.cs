using System;
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

    public override void Start() {
        base.Start();
        _maxHealth = _health; 
        _healthBarPrefab = Instantiate(_healthBarPrefab);
        _healthBarPrefab.Setup(transform);
        

    }
    
    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        _navMeshAgent.SetDestination(point); 
    }

    public void TakeDamage(int damageValue) {
        _health -= damageValue;
        _healthBarPrefab.SetHealth(_health,_maxHealth);
        if (_health <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        Debug.Log("Destroy");
        _serviceLocator = ServiceLocator.Instance;
        _management = _serviceLocator.Get<Management>(); 
        _management.Unselect(this);
    }
}
