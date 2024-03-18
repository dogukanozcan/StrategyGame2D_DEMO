using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Barracks : Building
{
    [HideInInspector] public bool canProduce;
    public List<BoardUnit> productList = new();
    public SpawnPoint spawnPoint;

    private Coroutine spawnPointColliderChecker;
    public override void OnMouseDown()
    {
        base.OnMouseDown();
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        spawnPoint.gameObject.SetActive(true);
    }

    public void ResetSelect()
    {
        spawnPoint.gameObject.SetActive(false);
    }
    
    public override void Placed(Tile originTile)
    {
        base.Placed(originTile);

        var spawnPointTile = GetEligibleSpawnPoint();
        SetSpawnPoint(spawnPointTile);
    }

    public void SetSpawnPoint(Tile spawnPointTile)
    {
        if (spawnPointTile == null)
        {  //CheckNearst tile is exist
            spawnPointTile = GetNearstEmptyTileSpawnPoint();
        }

        if (spawnPointTile != null)
        {
            //spawnPoint.gameObject.SetActive(false);
            spawnPoint.transform.position = spawnPointTile.transform.position;
            spawnPoint.Placed(spawnPointTile);
        }
        else
        {  
            //Hide spawn point
            spawnPoint.gameObject.SetActive(false);
            spawnPoint.transform.position = Vector3.zero;
        }

        //StartColliderCheck AFTER SPAWNED
        if (spawnPointColliderChecker == null)
        {
            spawnPointColliderChecker = StartCoroutine(SpawnPointColliderCheck());
        }
    }
    private IEnumerator SpawnPointColliderCheck()
    {
        while (true)
        {
            yield return new WaitWhile(() => spawnPoint.originTile.isEmpty);
            SetSpawnPoint(GetNearstEmptyTileSpawnPoint());
            yield return new WaitForFixedUpdate();
        }
        
    }
    private Tile GetNearstEmptyTileSpawnPoint()
    {
        return BoardManager.Instance.board.GetNearstEmptyTile(spawnPoint.originTile);
    }

    private Tile GetEligibleSpawnPoint()
    {
        if(originTile == null)
        {
            Debug.LogError("GetEligibleSpawnPoint() => originTile not found!");
            return null;
        }

        //Left to Right | Bottom Edge
        int startX = originTile.index.x;
        int startY = originTile.index.y - 1;
        for (int x = startX; x < startX + dimension.x + 1; x++)
        {
            var nextTile = BoardManager.Instance.board.GetTile(x, startY);
            if (nextTile != null && nextTile.isEmpty)
            {
                canProduce = true;
                return nextTile;
            }
        }

        //Bottom to Top | Right Edge
        startX = originTile.index.x + (int)dimension.x;
        startY = originTile.index.y;
        for (int y = startY; y < startY + dimension.y + 1; y++)
        {
            var nextTile = BoardManager.Instance.board.GetTile(startX, y);
            if (nextTile != null && nextTile.isEmpty)
            {
                canProduce = true;
                return nextTile;
            }
        }

        //Right to Left | Top Edge
        startX = originTile.index.x + (int)dimension.x;
        startY = originTile.index.y + (int)dimension.y + 1;
        for (int x = startX; x >= originTile.index.x-1; x--)
        {
            var nextTile = BoardManager.Instance.board.GetTile(x, startY);
            if (nextTile != null && nextTile.isEmpty)
            {
                canProduce = true;
                return nextTile;
            }
        }

        //Top to Bottom | Left Edge
        startX = originTile.index.x - 1;
        startY = originTile.index.y + (int)dimension.y;
        for (int y = startY; y <= originTile.index.y-1; y--)
        {
            var nextTile = BoardManager.Instance.board.GetTile(startX, y);
            if (nextTile != null && nextTile.isEmpty)
            {
                canProduce = true;
                return nextTile;
            }
        }

        canProduce = false;
        return null;

    }
}
