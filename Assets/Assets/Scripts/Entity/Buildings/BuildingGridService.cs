using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

//todo: рефактор
public class BuildingGridHelper
{
    private Grid _grid;
    private Tilemap _higlightTilemap;
    private Tilemap _buildingTilemap;
    
    private TileBase _redTile;
    private TileBase _greenTile;
    private TileBase _occupiedTile;

    public BuildingGridHelper(GridData data)
    {
        _grid = data.Grid;
        _higlightTilemap = data.HiglightTilemap;
        _buildingTilemap = data.BuildingTilemap;
        _redTile = data.RedTile;
        _greenTile = data.GreenTile;
        _occupiedTile = data.OccupiedTile;
    }

    public void DragBuilding(Building building)
    {
        Vector3Int cellPosition = GetGridPositionUnderMouse();
        cellPosition.z = 0;
        building.transform.position = _grid.CellToWorld(cellPosition);

        if (building.TryGetComponent<DraggableBuilding>(out _))
        {
            UpdateCellHighlight(
                _grid.WorldToCell(building.transform.position), 
                building
            );
        }
    }
    public void UpdateCellHighlight(Vector3Int cellPosition, Building building)
    {
        _higlightTilemap.ClearAllTiles();
        for (int i = 0; i < building.Heigth; i++)
        {
            for (int j = 0; j < building.Width; j++)
            {
                var position = new Vector3Int(cellPosition.x + j, cellPosition.y + i, 0);
                var tile = _buildingTilemap.HasTile(position)
                    ? _redTile
                    : _greenTile;
                _higlightTilemap.SetTile(position, tile);
            }
        }
    }
    
    public void SetRectangleTile(int width, int height, Vector3Int cellPosition, TileBase tile, Tilemap tilemap)
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

    public bool HasTileOnRectangle(int width, int height, Vector3Int cellPosition, Tilemap tilemap, TileBase tile = null)
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
    
    public Vector3Int GetGridPositionUnderMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return _grid.WorldToCell(clickPosition);
    }

    public bool TryToPlace(Building building, Vector3 startPosition)
    {
        if (HasTileOnRectangle(building.Width,
                building.Heigth, _grid.WorldToCell(building.transform.position), _buildingTilemap))
        {
            building.transform.position = startPosition;
            SetBuilding(building, startPosition);
            _higlightTilemap.ClearAllTiles();
            return true;
        }
        else
        {
            ClearBuilding(building);
            SetBuilding(building, building.transform.position);
            _higlightTilemap.ClearAllTiles();
            return false;
        }
    }

    public void SetBuilding(Building building, Vector3 position)
    {
        var worldPosition = _grid.WorldToCell(position);
        SetRectangleTile(
            building.Width,
            building.Heigth,
            worldPosition,
            _occupiedTile,
            _buildingTilemap
        );
    }

    public void ClearBuilding(Building building)
    {
        var worldPosition = _grid.WorldToCell(building.transform.position);
        SetRectangleTile(
            building.Width,
            building.Heigth,
            worldPosition,
            null,
            _buildingTilemap
        );
    }
    
    public void ClearBuilding(Building building, Vector3 position)
    {
        SetRectangleTile(
            building.Width,
            building.Heigth,
            _grid.WorldToCell(position),
            null,
            _buildingTilemap
        );
    }

    public void HideHiglight()
    {
        _higlightTilemap.ClearAllTiles();
    }
    
    public void SetBuildToNearPosition(Building building, Vector3 vector)
    {
        var cellPosition = _grid.WorldToCell(vector);
        var maxRadius = 100; //todo вынести сделать под max XY карты
        for (int radius = 1; radius <= maxRadius; radius++)
        {
            foreach (var (x, y) in GetCellsAroundRectangle(cellPosition.x, cellPosition.y, building.Width, building.Heigth, radius))
            {
                if (!HasTileOnRectangle(building.Width, building.Heigth, new Vector3Int(x, y, 0), _buildingTilemap))
                {
                    building.transform.position = new Vector3Int(x, y, 0);
                    SetBuilding(building, new Vector3Int(x, y, 0));
                    return;
                }
            }
        }
    }
    
    IEnumerable<(int, int)> GetCellsAroundRectangle(int rectX, int rectY, int rectWidth, int rectHeight, int radius)
    {
        // Верхняя и нижняя границы (расширяем по радиусу)
        for (int x = rectX - radius+1; x <= rectX + rectWidth - 1 + radius; x++)
        {
            yield return (x, rectY - radius+1); // Нижняя граница
            yield return (x, rectY + rectHeight - 1 + radius); // Верхняя граница
        }

        // Левая и правая границы (исключаем углы, чтобы не дублировать)
        for (int y = rectY - radius + 1; y <= rectY + rectHeight - 2 + radius; y++)
        {
            yield return (rectX - radius, y); // Левая граница
            yield return (rectX + rectWidth - 1 + radius, y); // Правая граница
        }
    }
    public class GridData
    {
        public Grid Grid;
        public Tilemap HiglightTilemap;
        public Tilemap BuildingTilemap;
    
        public TileBase RedTile;
        public TileBase GreenTile;
        public TileBase OccupiedTile;

        public GridData(Grid grid, Tilemap higlightTilemap, Tilemap buildingTilemap, TileBase redTile, TileBase greenTile, TileBase occupiedTile)
        {
            Grid = grid;
            HiglightTilemap = higlightTilemap;
            BuildingTilemap = buildingTilemap;
            RedTile = redTile;
            GreenTile = greenTile;
            OccupiedTile = occupiedTile;
        }
    }
}
