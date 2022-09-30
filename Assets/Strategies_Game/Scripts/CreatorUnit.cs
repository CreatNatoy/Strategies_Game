using System.Collections.Generic;
using UnityEngine;

public class CreatorUnit : MonoBehaviour
{
    [Header("Knight")]
    [SerializeField] private Unit _prefabKnight;
    [SerializeField] private int _countKnight = 1;
    [SerializeField] private Transform _transformKnightPool;
    [SerializeField] private bool _autoExpendKnight;

    [Header("Enemy")]
    [SerializeField] private Enemy _prefabEnemy;
    [SerializeField] private int _countEnemy = 1;
    [SerializeField] private Transform _transformEnemyPool;
    [SerializeField] private bool _autoExpendEnemy; 

    private PoolMono<Unit> _poolMonoKnight;
    private PoolMono<Enemy> _poolMonoEnemy; 

    private ServiceLocator _serviceLocator; 
    
    private void Awake() {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.Register(this);
        
        _poolMonoKnight = new PoolMono<Unit>(_prefabKnight, _countKnight, _transformKnightPool, _autoExpendKnight);
        _poolMonoEnemy = new PoolMono<Enemy>(_prefabEnemy, _countEnemy, _transformEnemyPool, _autoExpendEnemy); 
    }

    public Unit GetKnight() => _poolMonoKnight.GetFreeElement();

    public List<Unit> GetAllActivateKnight() => _poolMonoKnight.GetAllActiveElements();

    public Enemy GetEnemy() => _poolMonoEnemy.GetFreeElement();

    public List<Enemy> GetAllActivateEnemy() => _poolMonoEnemy.GetAllActiveElements();
}
