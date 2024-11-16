using Assets.Scripts.Entity.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ObjectMoveService : MonoBehaviour //Возможно он не раигрует на нарисованные заранее тайлы
{
    public Grid grid;                       
    public Tilemap tilemap;                 
    public Tilemap tilemap2;                 
    public TileBase greenTile;              
    public TileBase redTile;
    public TileBase occupiedTile;                
    private GameObject selectedBuilding;    
    private IDraggable selectedDraggable;      
    private Vector3Int currentCellPosition;   
    private bool isDragging = false;          

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
                UpdateCellHighlight(cellPosition, building);
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
            selectedDraggable = draggable;
            selectedDraggable.Drag();
            selectedBuilding = hitCollider.gameObject;
        }
    }

    private void OnDragEnd()
    {
        if (selectedBuilding != null)
        {
            Vector3Int cellPosition = GetGridPositionUnderMouse();
            PlaceBuilding(cellPosition);
            ClearCellHighlight();
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
        ClearCellHighlight();
        for (int i = 0; i < building.Width; i++)
        {
            for (int j = 0; j < building.Heigth; j++)
            {
                var position = new Vector3Int(cellPosition.x - j, cellPosition.y - i - 2, 0);
                var tile = tilemap2.HasTile(position)
                    ? redTile
                    : greenTile;
                tilemap.SetTile(position, tile);
            }
        }
    }

    private void ClearCellHighlight()
    {
        tilemap.ClearAllTiles();
    }

    private void PlaceBuilding(Vector3Int cellPosition)
    {
        selectedBuilding.transform.position = grid.CellToWorld(cellPosition) + grid.cellSize / 2;
        MarkCellAsOccupied(cellPosition);
    }

    private void MarkCellAsOccupied(Vector3Int cellPosition)
    {
        if (selectedBuilding.TryGetComponent<Building>(out var building))
        {
            for (int i = 0; i < building.Width; i++)
            {
                for (int j = 0; j < building.Heigth; j++)
                {
                    var position = new Vector3Int(cellPosition.x - j, cellPosition.y - i - 2, 0);
                    tilemap2.SetTile(position, occupiedTile);
                }
            }
        }
    }

    private void OnStartDragging()
    {
        Vector3Int cellPosition = GetGridPositionUnderMouse();
        cellPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);           
        selectedBuilding.transform.position = grid.CellToWorld(cellPosition) + grid.cellSize / 2;
        if (selectedBuilding != null && selectedBuilding.TryGetComponent<Building>(out var building))
        {
            for (int i = 0; i < building.Width; i++)
            {
                for (int j = 0; j < building.Heigth; j++)
                {
                    var position = new Vector3Int(cellPosition.x - j, cellPosition.y - i - 2, 0);
                    tilemap2.SetTile(position, null);
                }
            }
        }
        isDragging = true;
    }
}
