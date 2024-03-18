using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    public static SoldierSpawner Instance;

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

    /// <summary>
    /// if Selected unit is barracks Spawn soldier at the spawn point
    /// </summary>
    /// <param name="soldier"></param>
    public void SpawnSoldierSelectedBoardUnit(Soldier soldier)
    {
        var boardUnit = BoardManager.Instance.selectedBoardUnit;
        if (boardUnit.GetType().Equals(typeof(Barracks)))
        {
            var tile = ((Barracks)boardUnit).spawnPoint.originTile;
            if (soldier != null && tile != null)
            {
                
                soldier.gameObject.SetActive(true);
                soldier.transform.position = tile.transform.position;
                soldier.Placed(tile);
            }
            else
            {
                Debug.LogError("soldier or tile is not found");
            }
        }
        else
        {
            Debug.LogError("selectedBoardUnit is not barracks");
        }

    }


}
