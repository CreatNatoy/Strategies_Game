using UnityEngine;

public class CreatorUnit : MonoBehaviour
{
    [Header("Knight")]
    [SerializeField] private Unit _prefabKnight;

    [SerializeField] private int _intCountKnight;
    [SerializeField] private Transform _transformKnightPool;
    [SerializeField] private bool _autoExpendKnight;

    private PoolMono<Unit> _poolMonoKnight;

    private ServiceLocator _serviceLocator; 
    
    private void Awake() {
        _serviceLocator = ServiceLocator.Instance;
        _serviceLocator.Register(this);
        
        _poolMonoKnight = new PoolMono<Unit>(_prefabKnight, _intCountKnight, _transformKnightPool, _autoExpendKnight);
    }

    public Unit GetKnight() => _poolMonoKnight.GetFreeElement(); 
}
