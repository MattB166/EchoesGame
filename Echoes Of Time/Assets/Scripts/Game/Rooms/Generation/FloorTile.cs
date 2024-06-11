using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorTile : RoomComponent
{
    public TileBase floorTile;

    public override void Generate(Vector2 position, Transform parent)
    {
        Tilemap floorTilemap = parent.GetComponent<Tilemap>();
        if(floorTilemap != null)
        {
            Vector3Int tilePos = floorTilemap.WorldToCell(position);
            floorTilemap.SetTile(tilePos, floorTile);
            floorTilemap.SetColliderType(tilePos,Tile.ColliderType.Grid);
        }
        
    }
}
