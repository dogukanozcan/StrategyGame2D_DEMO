using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoardUnitFactory : MonoBehaviour
{
    public static BoardUnitFactory Instance;

    public List<BoardUnit> boardUnitsPrefabs;
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

    public BoardUnit GetBoardUnit(string unitName)
    {
        return boardUnitsPrefabs.Find(a => a.unitName.RemoveSpace().Equals(unitName.RemoveSpace()));
    }

}

public static class UnitNameExtension
{
    public static string RemoveSpace(this string unitName)
    {
        return unitName.ToLower().Replace(" ", "");
    }
}
