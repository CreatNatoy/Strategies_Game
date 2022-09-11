using UnityEngine;

public class SelectableCollider : MonoBehaviour
{
    [SerializeField] private SelectableObject _selectableObject;

    public SelectableObject SelectableObject => _selectableObject; 
}
