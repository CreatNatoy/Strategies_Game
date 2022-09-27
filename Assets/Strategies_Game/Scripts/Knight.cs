using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    Idle,
    WalkToPoint,
    WalkToEnemy,
    Attack
}
public class Knight : Unit
{
        [SerializeField] private float _distanceToFollow = 7f;
        [SerializeField] private float _distanceToAttack = 1f;
        [SerializeField] private float _attackPeriod = 1f;
        [SerializeField] private int _damage = 1;
    
        private UnitState _currentUnitState;
        private Vector3 _targetPoint;
        private Enemy _targetEnemy;
        private float _timer;
    
        private void Update() {
            switch (_currentUnitState) {
                case UnitState.Idle:
                    FindClosestEnemy();
                    break;
                case UnitState.WalkToPoint:
                    FindClosestEnemy();
                    break;
                case UnitState.WalkToEnemy:
                    if (_targetEnemy) {
                        _navMeshAgent.SetDestination(_targetEnemy.transform.position);
                        var distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
    
                        if (distance > _distanceToFollow) {
                            SetState(UnitState.WalkToPoint);
                        }
    
                        if (distance < _distanceToAttack) {
                            SetState(UnitState.Attack);
                        }
                    }
                    else {
                        SetState(UnitState.WalkToPoint);
                    }
    
                    break;
                case UnitState.Attack:
                    _navMeshAgent.SetDestination(_targetEnemy.transform.position);

                    if (_targetEnemy) {
                        var distance = Vector3.Distance(transform.position, _targetEnemy.transform.position);
                        if (distance > _distanceToAttack) {
                            SetState(UnitState.WalkToEnemy);
                        }
    
                        _timer += Time.deltaTime;
                        if (_timer > _attackPeriod) {
                            _timer -= _attackPeriod;
                            _targetEnemy.TakeDamage(_damage);
                        }
                    }
                    else {
                        SetState(UnitState.WalkToPoint);
                    }
    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        public void SetState(UnitState unitState) {
            _currentUnitState = unitState;
    
            switch (_currentUnitState) {
                case UnitState.Idle:
                    break;
                case UnitState.WalkToPoint:
                    break;
                case UnitState.WalkToEnemy:
                    break;
                case UnitState.Attack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void FindClosestEnemy() {
            // Remove the method Find
            var allEnemies = FindObjectsOfType<Enemy>();
    
            var minDistance = Mathf.Infinity;
    
            foreach (var enemy in allEnemies) {
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
    
                if (distance < _distanceToFollow && distance < minDistance) {
                    minDistance = distance;
                    _targetEnemy = enemy;
                }
            }
    
            if (minDistance < _distanceToFollow) {
                SetState(UnitState.WalkToEnemy);
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
