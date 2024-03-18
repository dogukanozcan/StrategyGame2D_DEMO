using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    public static ProductionManager Instance;

    //public List<BoardUnit> productionPrefabs = new List<BoardUnit>();

    [HideInInspector] public BoardUnit selectedPlaceableUnit;

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
    /// selectedPlaceableUnit used by BoardManager
    /// </summary>
    /// <param name="unitName"></param>
    public void SetSelectedPlaceableUnit(BoardUnit boardUnit)
    {
        selectedPlaceableUnit = boardUnit;
    }

    public void ResetSelectedPlaceableUnit()
    {
        selectedPlaceableUnit = null;
        ProductionMenuUI.Instance.ResetSelectedPlaceableUnit();
    }



}
