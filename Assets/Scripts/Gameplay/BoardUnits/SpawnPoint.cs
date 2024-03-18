using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [HideInInspector] public Tile originTile;
    public void Placed(Tile originTile)
    {
        this.originTile = originTile;
    }
}
