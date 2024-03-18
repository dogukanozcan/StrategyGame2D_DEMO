using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Board
{
    private int width; 
    private int height;
    public int tileSize = 1;
    private Tile[,] board;

    private Vector3 worldBottomLeft;
    public Board(int width, int height, int tileSize = 1)
    {
        this.width = width;
        this.height = height;
        this.tileSize = tileSize;

        board = new Tile[width, height];

        worldBottomLeft = (-Vector3.right * (width* tileSize) / 2) + (-Vector3.up * (height*tileSize) / 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var tilePosition = new Vector3(i * tileSize + (tileSize / 2.0f), j * tileSize + (tileSize / 2.0f));
                var tile = BoardManager.Instance.CreateTile(worldBottomLeft + tilePosition);
                tile.SetupTile(new Vector2Int(i, j));
                board[i, j] = tile;
            }
        }
    }

    public Tile GetNeighbourEmptyTile(Tile originTile, Tile sourceTile)
    {
        var tileList = new List<Vector2Int>() { 
            new Vector2Int(1,0),
            new Vector2Int(1,1),
            new Vector2Int(0,1),
            new Vector2Int(-1,1),
            new Vector2Int(-1,0),
            new Vector2Int(-1,-1),
            new Vector2Int(0,-1),
            new Vector2Int(1,-1)
        };
        var minDistance = float.MaxValue;
        Tile minDistanceTile = null;
        foreach (var item in tileList)
        {
            if (originTile.GetNeighbourByDirection(item.x, item.y).isEmpty)
            {
                var target = originTile.GetNeighbourByDirection(item.x, item.y);
                var distance = sourceTile.TileDistance(target);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    minDistanceTile = target;
                }
                
            }
                
        }
        return minDistanceTile;
    }
    public Tile GetTileByWorldPosition(Vector3 worldPosition)
    {
        var tilePosition = new Vector3(worldPosition.x / tileSize - (tileSize / 2.0f), worldPosition.y / tileSize - (tileSize / 2.0f));
        tilePosition -= worldBottomLeft;

        return board[(int)tilePosition.x, (int)tilePosition.y];

    }

    /// <summary>
    /// Get Tile by x and y
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Tile GetTile(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return board[x, y];
        }
        else
        {
            return null;
        }
    }

  

    /// <summary>
    /// Gives the nearest empty Tile to the originTile
    /// </summary>
    /// <param name="originTile"></param>
    /// <returns></returns>
    public Tile GetNearstEmptyTile(Tile originTile)
    {
        Tile nearstTile = null;
        float nearstDistance = float.MaxValue;
        foreach (var item in board)
        {
            if (item.isEmpty)
            {
                var distance = Vector2Int.Distance(item.index, originTile.index);
                if(distance < nearstDistance)
                {
                    nearstDistance = distance;
                    nearstTile = item;
                }
            }
        }
        return nearstTile;
    }

   
}
