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

    private EnemyState _currentEnemyState;
    private Building _targetBuilding;
    private Unit _targetUnit;
    private float _timer;

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
                if (_targetUnit) {
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

                if (_targetUnit) {
                    var distance = Vector3.Distance(transform.position, _targetUnit.transform.position);
                    if (distance > _distanceToAttack) {
                        SetState(EnemyState.WalkToUnit);
                    }

                    _timer += Time.deltaTime;
                    if (_timer > _attackPeriod) {
                        _timer -= _attackPeriod;
                        _targetUnit.TakeDamage(_damage);
                    }
                }
                else {
                    SetState(EnemyState.WalkToBuilding);
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SetState(EnemyState enemyState) {
        _currentEnemyState = enemyState;

        switch (_currentEnemyState) {
            case EnemyState.Idle:
                break;
            case EnemyState.WalkToBuilding:
                FindClosestBuilding();
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
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
        var allBuildings = FindObjectsOfType<Building>();

        var minDistance = Mathf.Infinity;

        foreach (var building in allBuildings) {
            var distance = Vector3.Distance(transform.position, building.transform.position);

            if (distance < minDistance) {
                minDistance = distance;
                _targetBuilding = building;
            }
        }
    }

    public void FindClosestUnit() {
        // Remove the method Find
        var allUnits = FindObjectsOfType<Unit>();

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

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToAttack);

        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.up, _distanceToFollow);
    }
#endif
}