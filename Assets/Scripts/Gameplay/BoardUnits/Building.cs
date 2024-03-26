using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : BoardUnit
{
    public Tile GetNearstTileBySoldier(Soldier soldier)
    {
        var list = new List<Tile>();
        if (originTile == null)
        {
            Debug.LogError("GetNearstTileBySoldier() => originTile not found!");
            return null;
        }

        //Left to Right | Bottom Edge
        int startX = originTile.index.x;
        int startY = originTile.index.y - 1;
        for (int x = startX; x < startX + dimension.x + 1; x++)
        {
            var nextTile = BoardManager.Instance.board.GetTile(x, startY);
            if (nextTile == null)
                continue;

            if (nextTile.isEmpty || (!nextTile.isEmpty && nextTile == soldier.originTile))
            {
                list.Add(nextTile);
            }
        }

        //Bottom to Top | Right Edge
        startX = originTile.index.x + (int)dimension.x;
        startY = originTile.index.y;
        for (int y = startY; y < startY + dimension.y + 1; y++)
        {
            var nextTile = BoardManager.Instance.board.GetTile(startX, y);
            if (nextTile == null)
                continue;

            if (nextTile.isEmpty || (!nextTile.isEmpty && nextTile == soldier.originTile))
            {
                list.Add(nextTile);
            }
        }

        //Right to Left | Top Edge
        startX = originTile.index.x + (int)dimension.x;
        startY = originTile.index.y + (int)dimension.y;
        for (int x = startX; x >= originTile.index.x - 1; x--)
        {
            var nextTile = BoardManager.Instance.board.GetTile(x, startY);
            if (nextTile == null)
                continue;

            if (nextTile.isEmpty || (!nextTile.isEmpty && nextTile == soldier.originTile))
            {
                list.Add(nextTile);
            }
        }

        //Top to Bottom | Left Edge
        startX = originTile.index.x-1;
        startY = originTile.index.y + (int)dimension.y;
        for (int y = startY; y >= originTile.index.y - 1; y--)
        {
            var nextTile = BoardManager.Instance.board.GetTile(startX, y);
            if (nextTile == null)
                continue;

            if (nextTile.isEmpty || (!nextTile.isEmpty && nextTile == soldier.originTile))
            {
                list.Add(nextTile);
            }
        }

       
        //FIND NEARST
        float minDistance = float.MaxValue;
        Tile nearstTile = null;
        foreach (var item in list)
        {
            var distance = item.TileDistance(soldier.originTile);
            if(distance < minDistance)
            {
                minDistance = distance;
                nearstTile = item;
            }
        }
       
        return nearstTile;
    }
}
