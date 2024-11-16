using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class TilemapSetter : MonoBehaviour
{
    public Tilemap tilemap; 
    public Grid grid; 
    public TileBase greenTile; 
    void Start()
    {
       tilemap.SetTile(new Vector3Int(1,1,0), greenTile);
    }

    // Update is called once per frame
    private void Update()
    { 
        Vector3Int cellPosition = GetGridPositionUnderMouse();
        cellPosition = new Vector3Int(cellPosition.x-1, cellPosition.y, 0);
        tilemap.SetTile(cellPosition, greenTile);
    }
    
    private Vector3Int GetGridPositionUnderMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        var clickPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        return grid.WorldToCell(clickPosition);
    }
}
