using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum RoomType
{
    Start,
    BattleArena,
    Treasure,
    Boss,
    Empty,
    End,
}
public class Room : MonoBehaviour
{
   public RoomType roomType;
   public bool isCleared = false;
   public bool isVisited = false;
   public bool isCurrent = false;
    public int width;
    public int height;
    public Tilemap tilemap;
    public TileBase floorTile;
    public TileBase leftWallTile;
    public TileBase rightWallTile;
    public TileBase topWallTile;
    public TileBase bottomWallTile;
    public TileBase[,] tiles;
    public Vector2Int exits;

    private void Start()
    {
        Room room = GenerateRoom(width, height, 1, RoomType.Empty);
        RenderRoom(room);
    }

    public Room GenerateRoom(int width, int height, int exits, RoomType type)
    {
        Room room = new Room
        { width = width, height = height, exits = new Vector2Int(0, 0), roomType = type, tiles = new TileBase[width,height] };

        InitialiseRoomTiles(room);

        PlaceExits(room, exits);

        return room;
    }

    private void InitialiseRoomTiles(Room room)
    {
        for(int x = 0; x < room.width; x++)
        {
            for(int y = 0; y < room.height; y++)
            {
              if(x == 0)
                {
                    room.tiles[x, y] = leftWallTile;
                }
               if(y == 0)
                {
                    room.tiles[x, y] = bottomWallTile;
                }
               if(y == 1)
                {
                    room.tiles[x, y] = floorTile;
                }
              else if(x == room.width -1)
                {
                    room.tiles[x, y] = rightWallTile;
              }
              else if(y == room.height - 1)
                {
                    room.tiles[x, y] = topWallTile;
              }
              
            }
        }

        CheckTiles(room);
    }

    private void CheckTiles(Room room)
    {
        for(int x = 0;x < room.width; x++)
        {
            for(int y = 0; y < room.height; y++)
            {

                if (room.tiles[x, y] == floorTile)
                {
                    bool hasBlockingNeighbour = false;

                    if(y < room.height - 1 && room.tiles[x, y + 1] == floorTile)
                    {
                        hasBlockingNeighbour = true;
                    }
                    if(y> 0 && room.tiles[x, y - 1] == floorTile)
                    {
                        hasBlockingNeighbour = true;
                    }
                    if(hasBlockingNeighbour)
                    {
                        room.tiles[x, y] = null;
                    }
                }
            }
        }
    }

    private void PlaceExits(Room room, int exits)
    {

    }


    public void RenderRoom(Room room)
    {
        for(int x = 0; x < room.width; x++)
        {
            for(int y = 0; y < room.height; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), room.tiles[x, y]);
            }
        }
    }

}
