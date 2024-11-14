using Assets.Scripts.Entity.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class ObjectMoveService : MonoBehaviour
{
    public Grid grid;                         // Объект сетки
    public Tilemap tilemap;                   // Tilemap для отображения доступных ячеек
    public TileBase greenTile;                // Плитка для подсветки доступных ячеек
    public TileBase redTile;                  // Плитка для подсветки занятых ячеек
    private GameObject selectedBuilding;      // Выделенное здание для перемещения
    private IDraggable selectedDraggable;      // Выделенное здание для перемещения
    private Vector3Int currentCellPosition;   // Позиция текущей ячейки
    private bool isDragging = false;          // Флаг для перетаскивания здания

    private PlayerControls controls; // Ссылка на Input Action

    private void Awake()
    {
        controls = new PlayerControls();

        // Подписываемся на действия Input System
        controls.Player.Click.performed += ctx => OnClick();
        controls.Player.Drag.performed += ctx => isDragging = true;
        controls.Player.Drag.canceled += ctx => OnDragEnd();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        Vector3Int cellPosition = GetGridPositionUnderMouse();
        tilemap.SetTile(cellPosition, greenTile);
        Debug.Log(cellPosition);

        /*if (isDragging && selectedBuilding != null)
        {
            Vector3Int cellPosition = GetGridPositionUnderMouse();
            selectedBuilding.transform.position = grid.CellToWorld(cellPosition) + grid.cellSize / 2;
            tilemap.SetTile(cellPosition, greenTile);
            UpdateCellHighlight(cellPosition);
        }*/
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
        /*Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 worldPosition = hitInfo.point;
            return grid.WorldToCell(worldPosition);
            
        }
        return Vector3Int.zero;*/

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return grid.WorldToCell(clickPosition);
    }

    private void UpdateCellHighlight(Vector3Int cellPosition)
    {
        ClearCellHighlight();
        bool canPlace = IsCellFree(cellPosition);
        TileBase tileToUse = canPlace ? greenTile : redTile;
        tilemap.SetTile(cellPosition, tileToUse);
    }

    private bool IsCellFree(Vector3Int cellPosition)
    {
        return tilemap.GetTile(cellPosition) == null;
    }

    private void ClearCellHighlight()
    {
        tilemap.ClearAllTiles();
    }

    private void PlaceBuilding(Vector3Int cellPosition)
    {
        if (IsCellFree(cellPosition))
        {
            selectedBuilding.transform.position = grid.CellToWorld(cellPosition) + grid.cellSize / 2;
            MarkCellAsOccupied(cellPosition);
        }
    }

    private void MarkCellAsOccupied(Vector3Int cellPosition)
    {
        TileBase occupiedTile = ScriptableObject.CreateInstance<Tile>();
        tilemap.SetTile(cellPosition, occupiedTile);
        transform.position.x = 2;
    }
}
