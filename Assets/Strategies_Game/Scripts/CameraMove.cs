using System;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Camera _raycastCamera;

    private Vector3 _startPoint;
    private Vector3 _cameraStartPosition;
    private Plane _plane;

    private void Start() {
        _plane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update() {
        HandlerPositionCamera();
    }

    private void HandlerPositionCamera() {
        
        var point = GetPointRaycast();

        if (Input.GetMouseButtonDown(2)) {
            _startPoint = point;
            _cameraStartPosition = transform.position;
        }

        if (Input.GetMouseButton(2)) {
            var offset = point - _startPoint;
            transform.position = _cameraStartPosition - offset;
        }

        var mouseScrollDelta = Input.mouseScrollDelta.y;
        transform.Translate(0f, 0f, mouseScrollDelta);
        _raycastCamera.transform.Translate(0f, 0f, mouseScrollDelta);
    }

    private Vector3 GetPointRaycast() {
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.green);

        _plane.Raycast(ray, out var distance);
        var point = ray.GetPoint(distance);

        return point;
    }
}
