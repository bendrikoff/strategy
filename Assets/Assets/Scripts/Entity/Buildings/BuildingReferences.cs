using System;
using Assets.Scripts.Entity.Buildings;
using Script.Architecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

//todo: рефактор
public class BuildingReferences: Singleton<BuildingReferences>
{
    [Header("Настройки Tilemap")]
    public Grid Grid;                       
    public Tilemap HiglightTilemap;                 
    public Tilemap BuildingTilemap;                 
    public TileBase GreenTile;              
    public TileBase RedTile;
    public TileBase OccupiedTile;
    public CameraController CameraController;

    public BuildingGridHelper.GridData GridData => new BuildingGridHelper.GridData(Grid,
        HiglightTilemap,
        BuildingTilemap,
        RedTile,
        GreenTile,
        OccupiedTile);
}
