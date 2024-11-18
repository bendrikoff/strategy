using Assets.Scripts.Entity.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ObjectMoveService : MonoBehaviour
{
    public Grid grid;                       
    public Tilemap HiglightTilemap;                 
    public Tilemap BuildingTilemap;                 
    public TileBase greenTile;              
    public TileBase redTile;
    public TileBase occupiedTile;                
    private GameObject selectedBuilding;    
    private IDraggable selectedDraggable;      
    private Vector3Int currentCellPosition;   
    private bool isDragging = false;
    private Vector3Int startPosition;

    private PlayerControls controls; // Ссылка на Input Action

    private void Awake()
    {
        controls = new PlayerControls();

        // Подписываемся на действия Input System
        controls.Player.Click.performed += ctx => OnClick();
        controls.Player.Drag.performed += ctx => OnStartDragging();
        controls.Player.Drag.canceled += ctx => OnDragEnd();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    { 
        if (isDragging && selectedBuilding != null)
        {
            Vector3Int cellPosition = GetGridPositionUnderMouse();
            cellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);           
            selectedBuilding.transform.position = grid.CellToWorld(cellPosition) + grid.cellSize / 2;
            if (selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
            {
                UpdateCellHighlight(grid.WorldToCell(building.TileStartCorner.position), building);
            }
        }
    }

    private void OnClick()
    {
        // Проверка клика на здание
        Vector3Int cellPosition = GetGridPositionUnderMouse();
        Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (hitCollider != null && hitCollider.TryGetComponent<IDraggable>(out var draggable))
        {
            startPosition = grid.WorldToCell(hitCollider.GetComponent<DraggableBuilding>().TileStartCorner.position); 
            selectedDraggable = draggable;
            selectedDraggable.Drag();
            selectedBuilding = hitCollider.gameObject;
        }
    }

    private void OnDragEnd()
    {
        
        if (selectedBuilding != null)
        {
            if (selectedBuilding.TryGetComponent<DraggableBuilding>(out var building) && HasTileOnRectangle(building.Width,
                    building.Heigth, grid.WorldToCell(building.TileStartCorner.position), BuildingTilemap))
            {
                selectedBuilding.transform.position = grid.CellToWorld(startPosition); //Тут происходит неправильное перемещениеы 
                PlaceBuilding(startPosition);
            }
            else
            {
                PlaceBuilding(grid.WorldToCell(building.TileStartCorner.position));
            }
            
            HiglightTilemap.ClearAllTiles();
            isDragging = false;
            selectedBuilding = null;
            selectedDraggable.UnDrag();
        }
    }

    private Vector3Int GetGridPositionUnderMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return grid.WorldToCell(clickPosition);
    }

    private void UpdateCellHighlight(Vector3Int cellPosition, Building building)
    {
        HiglightTilemap.ClearAllTiles();
        for (int i = 0; i < building.Width; i++)
        {
            for (int j = 0; j < building.Heigth; j++)
            {
                var position = new Vector3Int(cellPosition.x + j, cellPosition.y + i, 0);
                var tile = BuildingTilemap.HasTile(position)
                    ? redTile
                    : greenTile;
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
        if (selectedBuilding.TryGetComponent<Building>(out var building))
        {
            SetRectangleTile(building.Width, building.Heigth, startPosition, null, BuildingTilemap);
            SetRectangleTile(building.Width, building.Heigth, cellPosition, occupiedTile,BuildingTilemap);
        }
    }

    private void OnStartDragging()
    {
        isDragging = true;
        if (selectedBuilding != null && selectedBuilding.TryGetComponent<DraggableBuilding>(out var building))
        {
            SetRectangleTile(building.Width, building.Heigth,
                grid.WorldToCell(building.TileStartCorner.position), null, BuildingTilemap);
        }
    }

    private void SetRectangleTile(int width, int height, Vector3Int cellPosition, TileBase tile, Tilemap tilemap)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var position = new Vector3Int(cellPosition.x + j, cellPosition.y + i, 0);
                tilemap.SetTile(position, tile);
            }
        }
    }

    private bool HasTileOnRectangle(int width, int height, Vector3Int cellPosition, Tilemap tilemap, TileBase tile = null)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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
