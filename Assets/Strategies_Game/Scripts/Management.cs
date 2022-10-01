using System;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Image _frameImage;

    private Camera _camera;
    private SelectionState _currentSelectionState;
    private SelectableObject _hovered;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;
    private List<SelectableObject> _listOfSelected = new List<SelectableObject>();
    private CreatorUnit _creatorUnit; 

    private void Awake() {
       ServiceLocator.Instance.Register(this);
    }

    private void Start() {
        _creatorUnit = ServiceLocator.Instance.Get<CreatorUnit>();
        _camera = Camera.main;
    }

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

                    var rowNumber = Mathf.CeilToInt(Mathf.Sqrt(_listOfSelected.Count));
                    
                    for (var i = 0; i < _listOfSelected.Count; i++) {
                        
                        var row = i / rowNumber;
                        var column = i % rowNumber; 
                        
                        var point = hit.point + new Vector3(row, 0f, column);
                        
                        _listOfSelected[i].WhenClickOnGround(point);
                    }
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
        var allUnits = _creatorUnit.GetAllActivateKnight();

        foreach (var unit in from unit in allUnits let screenPosition = _camera.WorldToScreenPoint(unit.transform.position) where rect.Contains(screenPosition) select unit) {
            Select(unit);
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

    public void Unselect(SelectableObject selectableObject) {
        if (_listOfSelected.Contains(selectableObject)) {
            _listOfSelected.Remove(selectableObject);
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