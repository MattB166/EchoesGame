using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallTile : RoomComponent
{
    public TileBase wallTile;
    public override void Generate(Vector2 position, Transform parent)
    {
        Tilemap wallTilemap = parent.GetComponent<Tilemap>();
        if(wallTilemap != null)
        {
            Vector3Int tilePos = wallTilemap.WorldToCell(position);
            wallTilemap.SetTile(tilePos, wallTile);
            wallTilemap.SetColliderType(tilePos,Tile.ColliderType.Grid);
        }
        
    }
}
