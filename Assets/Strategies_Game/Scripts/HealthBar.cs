using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _scaleTransform;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private float _offsetUp = 2f; 

    private Transform _cameraTransform;

    private void Start() {
        _cameraTransform = Camera.main.transform; 
    }

    private void LateUpdate() {
        transform.position = _targetTransform.position + Vector3.up * _offsetUp;
        transform.rotation = _cameraTransform.rotation;
    }

    public void Setup(Transform target) {
        _targetTransform = target; 
        transform.SetParent(target);
    }

    public void SetHealth(int health, int maxHealth) {
        var xScale = (float)health / maxHealth;
        xScale = Mathf.Clamp01(xScale);
        _scaleTransform.localScale = new Vector3(xScale, 1f, 1f);
    }
}
