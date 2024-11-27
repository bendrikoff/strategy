using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Buildings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class BuildingGridHelper
{
    private Grid _grid;
    private Tilemap _higlightTilemap;
    private Tilemap _buildingTilemap;
    
    private TileBase _redTile;
    private TileBase _greenTile;
    private TileBase _occupiedTile;

    public BuildingGridHelper(Grid grid, Tilemap higlightTilemap, Tilemap buildingTilemap, TileBase redTile, TileBase greenTile, TileBase occupiedTile)
    {
        _grid = grid;
        _higlightTilemap = higlightTilemap;
        _buildingTilemap = buildingTilemap;
        _redTile = redTile;
        _greenTile = greenTile;
        _occupiedTile = occupiedTile;
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
}
