using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public string unitName;
    public Transform parent;
    public BoardUnit prefab;
    public List<BoardUnit> pool = new List<BoardUnit>();

    public Action CreateUnit;

    public BoardUnit GetNext()
    {
        var next = pool.Find(x => !x.gameObject.activeInHierarchy);
        if(next == null)
        {
            CreateUnit();
            next = pool.Find(x => !x.gameObject.activeInHierarchy);
        }
        return next;
    }
   
}
public class BoardUnitObjectPool : MonoBehaviour
{
    public static BoardUnitObjectPool Instance;

    public List<Pool> pools = new List<Pool>();
   
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

    private void Start()
    {
        var factory = BoardUnitFactory.Instance;
        InitPool(factory.GetBoardUnit("PowerPlant"), 20);
        InitPool(factory.GetBoardUnit("Barracks"), 20);
        InitPool(factory.GetBoardUnit("House"), 20);
        InitPool(factory.GetBoardUnit("Wall"), 20);
        InitPool(factory.GetBoardUnit("Hospital"), 20);
        InitPool(factory.GetBoardUnit("WatchTower"), 20);

        InitPool(factory.GetBoardUnit("Soldier1"), 20);
        InitPool(factory.GetBoardUnit("Soldier2"), 20);
        InitPool(factory.GetBoardUnit("Soldier3"), 20);
    }

    public void InitPool(BoardUnit prefab, int amount)
    {
        Transform _parent = new GameObject(prefab.unitName + " pool").transform;
        var pool = new Pool() { 
            parent = _parent ,
            unitName = prefab.unitName,
        };

        pool.CreateUnit = () =>
        {
            var boardUnit = Instantiate(prefab, _parent);
            boardUnit.gameObject.SetActive(false);
            pool.pool.Add(boardUnit);
        };

        for (int i = 0; i < amount; i++)
        {
            pool.CreateUnit();
        }
        pools.Add(pool);
    }

    public BoardUnit GetNextBoardUnit(string unitName)
    {
        var unit = pools.Find(pool => pool.unitName.RemoveSpace() == unitName.RemoveSpace()).GetNext();
        return unit;
    }
}
