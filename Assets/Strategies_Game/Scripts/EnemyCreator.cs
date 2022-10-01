using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private int _sizeEnemy = 3;

    private CreatorUnit _creatorUnit; 
    
    private void Start() {
        _creatorUnit = ServiceLocator.Instance.Get<CreatorUnit>(); 
        CreateEnemy();
    }

    private void CreateEnemy() {
        for (var i = 0; i < _sizeEnemy; i++) {
            var enemy = _creatorUnit.GetEnemy();
            enemy.ResetHealth();
            
            var position = _spawnTransform.position +
                           new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
            enemy.gameObject.transform.position = position;
        }
    }
}