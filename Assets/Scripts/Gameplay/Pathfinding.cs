using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        List<Tile> openSet = new List<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();
        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tile tile = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F <= tile.F)
                {
                    if (openSet[i].H < tile.H)
                        tile = openSet[i];
                }
            }

            openSet.Remove(tile);
            closedSet.Add(tile);

            if (tile == targetTile)
            {
                //Retrace path
                List<Tile> path = new List<Tile>();
                Tile current = targetTile;

                while (current != startTile)
                {
                    path.Add(current);
                    current = current.parent;
                }
                path.Reverse();

               
                return path;
            }

            foreach (Tile neighbour in GetWalkableAdjacentSquares(tile))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = tile.G + GetDistance(tile, neighbour);
                if (newCostToNeighbour < neighbour.G || !openSet.Contains(neighbour))
                {
                    neighbour.G = newCostToNeighbour;
                    neighbour.H = GetDistance(neighbour, targetTile);
                    neighbour.parent = tile;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }



    private int GetDistance(Tile tileA, Tile tileB)
    {
        return (int)(Vector2Int.Distance(tileA.index,tileB.index) * 10f);    
    }
  
    static List<Tile> GetWalkableAdjacentSquares(Tile tile)
    {
        var list = new List<Tile>();
        var ortogonal = new List<Vector2Int>()
        {
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.right,
        };  
        var diagonal = new List<Vector2Int>()
        {
            Vector2Int.right + Vector2Int.up,
            Vector2Int.right + Vector2Int.down,
            Vector2Int.left + Vector2Int.up,
            Vector2Int.left + Vector2Int.down,
        };


        foreach ( var item in ortogonal)
        {
            var neighbour = tile.GetNeighbourByDirection(item.x, item.y);
            if (neighbour && neighbour.isEmpty)
            {
                list.Add(neighbour);
            }
        }
        foreach (var item in diagonal)
        {
            var neighbour = tile.GetNeighbourByDirection(item.x, item.y);
            if (neighbour && neighbour.isEmpty)
            {
                list.Add(neighbour);
            }
        }
        return list;

    }
}
