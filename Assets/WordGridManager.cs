using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapService
{
    public TilemapService(TilemapData data)
    {
        
    }
}

public class TilemapData
{
    public Grid Grid;                       
    public Tilemap HiglightTilemap;                 
    public Tilemap BuildingTilemap;                 
    public TileBase GreenTile;              
    public TileBase RedTile;
    public TileBase OccupiedTile;

    public TilemapData(Grid grid, Tilemap higlightTilemap, Tilemap buildingTilemap, TileBase greenTile, TileBase redTile, TileBase occupiedTile)
    {
        Grid = grid;
        HiglightTilemap = higlightTilemap;
        BuildingTilemap = buildingTilemap;
        GreenTile = greenTile;
        RedTile = redTile;
        OccupiedTile = occupiedTile;
    }
}
