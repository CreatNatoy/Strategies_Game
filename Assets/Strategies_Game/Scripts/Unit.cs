using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    public override void WhenClickOnGround(Vector3 point) {
        base.WhenClickOnGround(point);

        _navMeshAgent.SetDestination(point); 
    }
}
