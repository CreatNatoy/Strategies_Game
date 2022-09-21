using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other
}

public class Management : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Image _frameImage;

    private SelectableObject _hovered;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;
    private List<SelectableObject> _listOfSelected = new List<SelectableObject>();

   [SerializeField] private SelectionState _currentSelectionState; 

    private void Update() {
        RaycastToSelectable(out var hit);

        if (Input.GetMouseButtonUp(0)) {
            if (_hovered) {
                if (Input.GetKey(KeyCode.LeftControl) == false)
                    UnselectAll();
                _currentSelectionState = SelectionState.UnitsSelected; 
                Select(_hovered);
            }

            if (_currentSelectionState == SelectionState.UnitsSelected) {
                if (hit.collider.CompareTag("Ground")) {
                    foreach (var selectable in _listOfSelected)
                        selectable.WhenClickOnGround(hit.point);
                }
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
            _frameEnd = Input.mousePosition;

            var min = Vector2.Min(_frameStart, _frameEnd);
            var max = Vector2.Max(_frameStart, _frameEnd);
            var size = max - min;

            if (size.magnitude < 10) return;
            _frameImage.enabled = true;

            _frameImage.rectTransform.anchoredPosition = min;

            _frameImage.rectTransform.sizeDelta = size;

            var rect = new Rect(min, size);

            UnselectAll();
            SelectObjectToFrame(rect);
            _currentSelectionState = SelectionState.Frame;
        }

        if (Input.GetMouseButtonUp(0)) {
            _frameImage.enabled = false;
            if (_listOfSelected.Count > 0) {
                _currentSelectionState = SelectionState.UnitsSelected;
            }
            else {
                _currentSelectionState = SelectionState.Other; 
            }
        }
    }

    private void SelectObjectToFrame(Rect rect) {
        // Remove the Find method 
        var allUnits = FindObjectsOfType<Unit>();
        
        foreach (var unit in allUnits) {
            var screenPosition = _camera.WorldToScreenPoint(unit.transform.position);
            if (rect.Contains(screenPosition)) {
                Select(unit);
            }
        }
    }

    private void RaycastToSelectable(out RaycastHit hit) {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        if (Physics.Raycast(ray, out hit)) {
            var selectableCollider = hit.collider.GetComponent<SelectableCollider>();

            if (selectableCollider != null) {
                var hitSelectable = selectableCollider.SelectableObject;

                if (_hovered) {
                    if (_hovered != hitSelectable) {
                        _hovered.OnUnhover();
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
        _hovered = hitSelectable;
        _hovered.OnHover();
    }

    private void Select(SelectableObject selectableObject) {
        if (!_listOfSelected.Contains(selectableObject)) {
            _listOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    private void UnselectAll() {
        foreach (var selectable in _listOfSelected) {
            selectable.Unselect();
        }
        _currentSelectionState = SelectionState.Other; 

        _listOfSelected.Clear();
    }

    private void UnhoverCurrent() {
        if (_hovered) {
            _hovered.OnUnhover();
            _hovered = null;
        }
    }
}