using System;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [Header("Tile Prefab")] 
    public Tile tile;

    [Header("Tile Generator")] 
    public int row, column;

    [Header("Camera Pos")] 
    public Transform camPos;

    public Dictionary<Vector2, Tile> tiles;

    private void Start()
    {
        GenerateTile();
    }

    private void GenerateTile()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < row; ++x)
        {
            for (int y = 0; y < column; ++y)
            {
                var tileSet = Instantiate(tile, new Vector3(x, y), Quaternion.identity);
                var offset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                tileSet.Init(offset);

                tiles[new Vector2(x, y)] = tileSet;
            }
        }
        camPos.position = new Vector3((float)row / 2 - 0.5f, (float)column / 2 - 0.5f, -10);
    }

    public Tile GetTilePos(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }

        return null;
    }
}
