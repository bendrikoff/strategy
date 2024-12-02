using Assets.Scripts.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

//todo: вынести данные в отдельный класс данных WordGridData
public class BuildingPlayState : IState
{
    private readonly BuildingGridHelper _buildingGridHelper;

    private CameraController _cameraController;
    private DraggableBuilding _selectedBuilding;
    private PlayerControls _controls;
    
    private Vector3 _startPosition;
    private bool _isDragging;

    public BuildingPlayState(
        BuildingGridHelper.GridData gridData,
        CameraController cameraController)
    {
        _cameraController = cameraController;
        
        _buildingGridHelper = new BuildingGridHelper(gridData);
        _controls = new PlayerControls();
        InitializeControls();
    }

    public void Enter()
    {
        _controls.Enable();
        _cameraController.EnterBuildingMoveState();
    }
    
    public void Execute()
    {
        if (_isDragging && _selectedBuilding != null)
        {
            _buildingGridHelper.DragBuilding(_selectedBuilding);
        }
    }

    public void Exit()
    {
        _controls.Disable();
        _cameraController.EnterIdleState();
        _buildingGridHelper.HideHiglight();
    }

    private void InitializeControls()
    {
        _controls.Player.Click.performed += ctx => OnClick();
        _controls.Player.Drag.performed += ctx => StartDragging();
        _controls.Player.Drag.canceled += ctx => StopDragging();
    }

    private void OnClick()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (hitCollider != null && hitCollider.TryGetComponent<DraggableBuilding>(out var building))
        {
            _startPosition = building.transform.position; 
            _selectedBuilding = building;
            _selectedBuilding.Drag();
        }
    }
    private void StartDragging()
    {
        _isDragging = true;
        if (_selectedBuilding != null && _selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
        {
            _buildingGridHelper.ClearBuilding(building, building.transform.position);
        }
    }

    private void StopDragging()
    {
        if (_selectedBuilding == null) return;

        _buildingGridHelper.TryToPlace(_selectedBuilding, _startPosition);
        _isDragging = false;
        _selectedBuilding.UnDrag();
        _selectedBuilding = null;
    }
}
