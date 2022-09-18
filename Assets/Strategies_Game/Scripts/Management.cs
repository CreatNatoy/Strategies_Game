using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Management : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SelectableObject _howered;
    [SerializeField] private Image _frameImage;

    private Vector2 _frameStart;
    private Vector2 _frameEnd; 

    private List<SelectableObject> _listOfSelected = new List<SelectableObject>();

    private void Update() {
        
        RaycastToSelectable(out var hit);

        if (Input.GetMouseButtonUp(0)) {
            if (_howered) {
                if (Input.GetKey(KeyCode.LeftControl) == false)
                    UnselectAll();
                Select();
            }

            if (hit.collider.CompareTag("Ground")) {
                foreach (var selectable in _listOfSelected)
                    selectable.WhenClickOnGround(hit.point);
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            UnselectAll();
        }

        HandlerFrameSelected();
    }

    private void HandlerFrameSelected() {
        if (Input.GetMouseButtonDown(0)) {
            _frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            _frameImage.enabled = true;

            _frameEnd = Input.mousePosition;

            var min = Vector2.Min(_frameStart, _frameEnd);
            var max = Vector2.Max(_frameStart, _frameEnd);

            _frameImage.rectTransform.anchoredPosition = min;

            var size = max - min;
            _frameImage.rectTransform.sizeDelta = size;
        }

        if (Input.GetMouseButtonUp(0)) {
            _frameImage.enabled = false;
        }
    }

    private void RaycastToSelectable(out RaycastHit hit) {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        if (Physics.Raycast(ray, out hit)) {
            var selectableCollider = hit.collider.GetComponent<SelectableCollider>();

            if (selectableCollider != null) {
                var hitSelectable = selectableCollider.SelectableObject;

                if (_howered) {
                    if (_howered != hitSelectable) {
                        _howered.OnUnhover();
                        OnHover(hitSelectable);
                    }
                }
                else {
                    OnHover(hitSelectable);
                }
            }
            else {
                UnhoverCurrent();
            }
        }
        else {
            UnhoverCurrent();
        }
    }

    private void OnHover(SelectableObject hitSelectable) {
        _howered = hitSelectable;
        _howered.OnHover();
    }

    private void Select() {
        if (!_listOfSelected.Contains(_howered)) {
            _listOfSelected.Add(_howered);
            _howered.Select();
        }
    }

    private void UnselectAll() {
        foreach (var selectable in _listOfSelected) {
            selectable.Unselect();
        }

        _listOfSelected.Clear();
    }

    private void UnhoverCurrent() {
        if (_howered) {
            _howered.OnUnhover();
            _howered = null;
        }
    }
}