using Assets.Scripts.Entity.Buildings;
using Script.Architecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingMoverService: Singleton<BuildingMoverService>
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

    private void Awake()
    {
        base.Awake();
        _controls = new PlayerControls();

        _controls.Player.Click.performed += ctx => OnClick();
        _controls.Player.Drag.performed += ctx => OnStartDragging();
        _controls.Player.Drag.canceled += ctx => OnDragEnd();

        UIEvents.OnMoveBuildingButtonClick += building =>
        {
            _selectedBuilding = building;
            _isDragging = false;
        };
    }

    private void OnEnable()
    {
        _controls.Enable();
        CameraController.EnterBuildingMoveState();
    }

    private void OnDisable()
    {
        _controls.Disable();
        CameraController.EnterIdleState();
    }

    private void Update()
    { 
        if (_isDragging && _selectedBuilding != null)
        {
            Vector3Int cellPosition = GetGridPositionUnderMouse();
            cellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);
            _selectedBuilding.transform.position = Grid.CellToWorld(cellPosition);
            if (_selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
            {
                UpdateCellHighlight(Grid.WorldToCell(building.transform.position), building);
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
            if (HasTileOnRectangle(_selectedBuilding.Width,
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

    private Vector3Int GetGridPositionUnderMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return Grid.WorldToCell(clickPosition);
    }

    private void UpdateCellHighlight(Vector3Int cellPosition, Building building)
    {
        HiglightTilemap.ClearAllTiles();
        for (int i = 0; i < building.Heigth; i++)
        {
            for (int j = 0; j < building.Width; j++)
            {
                var position = new Vector3Int(cellPosition.x + j, cellPosition.y + i, 0);
                var tile = BuildingTilemap.HasTile(position)
                    ? RedTile
                    : GreenTile;
                HiglightTilemap.SetTile(position, tile);
            }
        }
    }

    private void PlaceBuilding(Vector3Int cellPosition)
    {
        MarkCellAsOccupied(cellPosition);
    }

    private void MarkCellAsOccupied(Vector3Int cellPosition)
    {
        if (_selectedBuilding.TryGetComponent<Building>(out var building))
        {
            SetRectangleTile(building.Width, building.Heigth, _startPosition, null, BuildingTilemap);
            SetRectangleTile(building.Width, building.Heigth, cellPosition, OccupiedTile, BuildingTilemap);
        }
    }

    private void OnStartDragging()
    {
        _isDragging = true;
        if (_selectedBuilding != null && _selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
        {
            SetRectangleTile(building.Width, building.Heigth,
                Grid.WorldToCell(building.transform.position), null, BuildingTilemap);
        }
    }

    private void SetRectangleTile(int width, int height, Vector3Int cellPosition, TileBase tile, Tilemap tilemap)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var position = new Vector3Int(cellPosition.x + j, cellPosition.y + i, 0);
                tilemap.SetTile(position, tile);
            }
        }
    }

    private bool HasTileOnRectangle(int width, int height, Vector3Int cellPosition, Tilemap tilemap, TileBase tile = null)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var position = new Vector3Int(cellPosition.x + j, cellPosition.y + i , 0);
                if (tile == null && tilemap.HasTile(position))
                {
                    return true;
                }

                if (tile != null && tilemap.GetTile(position) == tile)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
