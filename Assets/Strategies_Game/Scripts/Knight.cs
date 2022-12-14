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
        private CreatorUnit _creatorUnit;
        
        public override void Start() {
            base.Start();
            _creatorUnit = ServiceLocator.Instance.Get<CreatorUnit>();
        }

        private void Update() {
            HandlerState();
        }

        private void HandlerState() {
            switch (_currentUnitState) {
                case UnitState.Idle:
                    FindClosestEnemy();
                    break;
                case UnitState.WalkToPoint:
                    FindClosestEnemy();
                    break;
                case UnitState.WalkToEnemy:
                    WalkToEnemy();
                    break;
                case UnitState.Attack:

                    if (_targetEnemy.gameObject.activeSelf) {
                        Attack();
                    }
                    else {
                        SetState(UnitState.WalkToPoint);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WalkToEnemy() {
            if (_targetEnemy.gameObject.activeSelf) {
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
        }

        private void Attack() {
            _navMeshAgent.SetDestination(_targetEnemy.transform.position);

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
            var allEnemies = _creatorUnit.GetAllActivateEnemy();
            
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

        private void OnDisable() {
            SetState(UnitState.Idle);
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
