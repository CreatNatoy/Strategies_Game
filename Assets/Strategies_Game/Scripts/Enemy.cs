using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private int _health = 20;
    [SerializeField] private float _distanceToFollow = 7f;
    [SerializeField] private float _distanceToAttack = 1f;
    [SerializeField] private float _attackPeriod = 1f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private HealthBar _healthBarPrefab;

    private int _maxHealth; 
    private EnemyState _currentEnemyState;
    private Building _targetBuilding;
    private Unit _targetUnit;
    private float _timer;
    private CreatorUnit _creatorUnit;

    private void Awake() {
        _maxHealth = _health;
    }

    private void Start() {
        _healthBarPrefab = Instantiate(_healthBarPrefab);
        _healthBarPrefab.Setup(transform);
        
        _creatorUnit = ServiceLocator.Instance.Get<CreatorUnit>();
    }

    private void Update() {
        switch (_currentEnemyState) {
            case EnemyState.Idle:
                FindClosestUnit();
                break;
            case EnemyState.WalkToBuilding:
                FindClosestUnit();
                if (_targetBuilding == null) {
                    SetState(EnemyState.Idle);
                }

                break;
            case EnemyState.WalkToUnit:
                if (_targetUnit.gameObject.activeSelf) {
                    _navMeshAgent.SetDestination(_targetUnit.transform.position);
                    var distance = Vector3.Distance(transform.position, _targetUnit.transform.position);

                    if (distance > _distanceToFollow) {
                        SetState(EnemyState.WalkToBuilding);
                    }

                    if (distance < _distanceToAttack) {
                        SetState(EnemyState.Attack);
                    }
                }
                else {
                    SetState(EnemyState.WalkToBuilding);
                }

                break;
            case EnemyState.Attack:

                if (_targetUnit.gameObject.activeSelf) {
                    Attack();
                }
                else {
                    SetState(EnemyState.WalkToBuilding);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Attack() {
        _navMeshAgent.SetDestination(_targetUnit.transform.position);

        var distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
        if (distance > _distanceToAttack) {
            SetState(EnemyState.WalkToUnit);
        }

        _timer += Time.deltaTime;
        if (_timer > _attackPeriod) {
            _timer -= _attackPeriod;
            if (_targetUnit.TryKilled(_damage)) {
                SetState(EnemyState.Idle);
            }
        }
    }

    public void SetState(EnemyState enemyState) {
        _currentEnemyState = enemyState;

        switch (_currentEnemyState) {
            case EnemyState.Idle:
                break;
            case EnemyState.WalkToBuilding:
                FindClosestBuilding();
                if (_targetBuilding) {
                    _navMeshAgent.SetDestination(_targetBuilding.transform.position);
                }
                break;
            case EnemyState.WalkToUnit:
                break;
            case EnemyState.Attack:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void FindClosestBuilding() {
        // Remove the method Find
    /*    var allBuildings = FindObjectsOfType<Building>();

        var minDistance = Mathf.Infinity;

        foreach (var building in allBuildings) {
            var distance = Vector3.Distance(transform.position, building.transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                _targetBuilding = building;
            }
        }*/
    }

    private void FindClosestUnit() {
        var allUnits = _creatorUnit.GetAllActivateKnight();

        var minDistance = Mathf.Infinity;

        foreach (var unit in allUnits) {
            var distance = Vector3.Distance(transform.position, unit.transform.position);

            if (distance < _distanceToFollow && distance < minDistance) {
                minDistance = distance;
                _targetUnit = unit;
            }
        }

        if (minDistance < _distanceToFollow) {
            SetState(EnemyState.WalkToUnit);
        }
    }
    
    public void TakeDamage(int damageValue) {
        _health -= damageValue;
        _healthBarPrefab.SetHealth(_health,_maxHealth);
        if (_health <= 0) {
            Die();
        }
    }

    private void Die() {
        gameObject.SetActive(false);
    }

    public void ResetHealth() {
        _health = _maxHealth;
        _healthBarPrefab.SetHealth(_health,_maxHealth);
    }
    

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);

        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);
    }
#endif
}