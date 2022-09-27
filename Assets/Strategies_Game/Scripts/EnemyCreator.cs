using System;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private float _creationPeriod;
    [SerializeField] private Enemy _enemyPrefab;

    private float _timer;

    private void Update() {
        CreateEnemy();
    }

    private void CreateEnemy() {
        _timer += Time.deltaTime;
        if (_timer > _creationPeriod) {
            _timer -= _creationPeriod; 
            Instantiate(_enemyPrefab, _spawnTransform.position, _spawnTransform.rotation);
        }
    }
}
