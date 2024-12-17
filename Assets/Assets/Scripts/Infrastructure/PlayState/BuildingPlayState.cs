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
    }

    public void Enter()
    {
        _controls.Enable();
        _cameraController.EnterBuildingMoveState();
        SubscribeControls();
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
    
    private void SubscribeControls()
    {
        _controls.Player.Click.performed += OnClick;
        _controls.Player.Drag.performed += StartDragging;
        _controls.Player.Drag.canceled += StopDragging;
    }
    private void OnClick(InputAction.CallbackContext obj)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (hitCollider != null && hitCollider.TryGetComponent<DraggableBuilding>(out var building))
        {
            _startPosition = building.transform.position; 
            _selectedBuilding = building;
            _selectedBuilding.Drag();
        }
    }
    private void StartDragging(InputAction.CallbackContext obj)
    {
        _isDragging = true;
        if (_selectedBuilding != null && _selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
        {
            _buildingGridHelper.ClearBuilding(building, building.transform.position);
        }
    }

    private void StopDragging(InputAction.CallbackContext obj)
    {
        if (_selectedBuilding == null) return;

        _buildingGridHelper.TryToPlace(_selectedBuilding, _startPosition);
        _isDragging = false;
        _selectedBuilding.UnDrag();
        _selectedBuilding = null;
    }
}
