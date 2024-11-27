using Assets.Scripts.Entity.Buildings;
using Script.Architecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

//todo: рефактор
public class BuildingMoverService: MonoBehaviour
{
    [Header("Настройки Tilemap")]
    public Grid Grid;                       
    public Tilemap HiglightTilemap;                 
    public Tilemap BuildingTilemap;                 
    public TileBase GreenTile;              
    public TileBase RedTile;
    public TileBase OccupiedTile;


    [Space] public CameraController CameraController;

    public DraggableBuilding _selectedBuilding;
    private Vector3Int _currentCellPosition;
    private bool _isDragging = false;
    private Vector3Int _startPosition;

    private PlayerControls _controls;
    public BuildingGridHelper _buildingGridHelper;

    private void Awake()
    {
        PlayerControlsInit();
        _buildingGridHelper =
            new BuildingGridHelper(Grid, HiglightTilemap, BuildingTilemap, RedTile, GreenTile, OccupiedTile);
    }

    private void OnEnable()
    {
        UIEvents.OnMoveBuildingButtonClick += OnStartMove;
        _controls.Enable();
        CameraController.EnterBuildingMoveState();
    }

    private void OnDisable()
    {
        UIEvents.OnMoveBuildingButtonClick -= OnStartMove;
        _controls.Disable();
        CameraController.EnterIdleState();
    }

    private void Update()
    { 
        if (_isDragging && _selectedBuilding != null)
        {
            Vector3Int cellPosition = _buildingGridHelper.GetGridPositionUnderMouse();
            cellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);
            _selectedBuilding.transform.position = Grid.CellToWorld(cellPosition);
            if (_selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
            {
                _buildingGridHelper.UpdateCellHighlight(Grid.WorldToCell(building.transform.position), building);
            }
        }
    }

    private void OnClick()
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (hitCollider != null && hitCollider.TryGetComponent<DraggableBuilding>(out var building) && building == _selectedBuilding)
        {
            _startPosition = Grid.WorldToCell(building.transform.position); 
            _selectedBuilding = building;
            _selectedBuilding.Drag();
        }
    }

    private void OnDragEnd()
    {
        
        if (_selectedBuilding != null)
        {
            if (_buildingGridHelper.HasTileOnRectangle(_selectedBuilding.Width,
                    _selectedBuilding.Heigth, Grid.WorldToCell(_selectedBuilding.transform.position), BuildingTilemap))
            {
                _selectedBuilding.transform.position = Grid.CellToWorld(_startPosition);
                PlaceBuilding(_startPosition);
            }
            else
            {
                PlaceBuilding(Grid.WorldToCell(_selectedBuilding.transform.position));
            }
            
            HiglightTilemap.ClearAllTiles();
            _isDragging = false;
            _selectedBuilding.UnDrag();
            _selectedBuilding = null;
        }
    }

    private void PlaceBuilding(Vector3Int cellPosition)
    {
        _buildingGridHelper.SetRectangleTile(_selectedBuilding.Width, _selectedBuilding.Heigth, _startPosition, null, BuildingTilemap);
        _buildingGridHelper.SetRectangleTile(_selectedBuilding.Width, _selectedBuilding.Heigth, cellPosition, OccupiedTile, BuildingTilemap);
    }
    private void OnDragging()
    {
        _isDragging = true;
        if (_selectedBuilding != null && _selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
        {
            _buildingGridHelper.SetRectangleTile(building.Width, building.Heigth,
                Grid.WorldToCell(building.transform.position), null, BuildingTilemap);
        }
    }

    private void PlayerControlsInit()
    {
        _controls = new PlayerControls();

        _controls.Player.Click.performed += ctx => OnClick();
        _controls.Player.Drag.performed += ctx => OnDragging();
        _controls.Player.Drag.canceled += ctx => OnDragEnd();
    }
    private void OnStartMove(DraggableBuilding building)
    {
        _selectedBuilding = building;
        _isDragging = false;
    }
}
