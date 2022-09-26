using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [SerializeField] private GameObject _selectionIndicator;
    [SerializeField] private float _scaleObject = 1.1f;

    public virtual void Start() {
        _selectionIndicator.SetActive(false);
    }

    public virtual void OnHover() {
        transform.localScale = Vector3.one * _scaleObject;
    }

    public virtual void OnUnhover() {
        transform.localScale = Vector3.one;
    }

    public virtual void Select() {
        _selectionIndicator.SetActive(true);
    }

    public virtual void Unselect() {
        _selectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point) {
        
    }
}