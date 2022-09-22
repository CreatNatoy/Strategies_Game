using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private int _price = 5;

    public int Price => _price; 
    
    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        _navMeshAgent.SetDestination(point); 
    }
}
