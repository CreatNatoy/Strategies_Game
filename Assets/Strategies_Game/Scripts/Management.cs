using UnityEngine;

public class Management : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableObject _howered;

    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        if (Physics.Raycast(ray, out var hit))
        {
            var selectableCollider = hit.collider.GetComponent<SelectableCollider>();
            if (selectableCollider != null)
            {
                _howered = selectableCollider.SelectableObject;
            }
        }
    }
}