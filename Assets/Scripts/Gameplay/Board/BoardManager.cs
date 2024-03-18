using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [HideInInspector] public Board board;
    public Tile tilePrefab;
    public Transform boardParent;
    public LayerMask tileMask;

    public Camera mainCamera;

    [SerializeField] private int boardWidth;
    [SerializeField] private int boardHeight;

    [HideInInspector] public BoardUnit selectedBoardUnit;
    [HideInInspector] public bool spawnPointChange = false;
    //[HideInInspector] public Barracks selectedBarracks;
    private void Awake()
    {
        if(Instance == null)
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
        board = new Board(boardWidth, boardHeight);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (ProductionManager.Instance.selectedPlaceableUnit != null)
            {
                var tile = GetTileMouseOver();
                if (tile != null)
                {
                    if (CheckPlaceable(ProductionManager.Instance.selectedPlaceableUnit, tile))
                    {
                        PlaceSelectedUnitToTile(tile);
                    }
                }
            }
            else if (spawnPointChange)
            {
                var tile = GetTileMouseOver();
                if (tile != null && tile.isEmpty)
                {
                    Barracks barracks = (Barracks)selectedBoardUnit;
                    barracks.SetSpawnPoint(tile);
                    InformationPanelUI.Instance.ChangeSpawnPointStatus(false);
                }
            }
           
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (selectedBoardUnit == null)
                return;

            //SoldierSelected
            if (selectedBoardUnit.GetType() == typeof(Soldier))
            {
                //Tile Selected
                var tile = GetTileMouseOver();
                if (tile == null)
                {
                    var boardUnit = GetBoardUnitMouseOver();
                    if (boardUnit.GetType().Equals(typeof(Soldier)))
                    {
                        //Soldier Selected
                        var targetSoldier = (Soldier)boardUnit;
                        var selectedSoldier = (Soldier)selectedBoardUnit;
                        selectedSoldier.SetTarget(targetSoldier);
                        tile = board.GetNeighbourEmptyTile(boardUnit.originTile,selectedBoardUnit.originTile);

                    }
                    else
                    {
                        //BUILDING CLICKED
                        tile = board.GetTileByWorldPosition(MousePositionToWorldPosition());
                        if (!tile.isEmpty)
                        {
                            tile = board.GetNearstEmptyTile(tile);
                            var selectedSoldier = (Soldier)selectedBoardUnit;
                            selectedSoldier.SetTarget(boardUnit);
                        }
                    }

                  
                }

                
                var soldier = (Soldier)selectedBoardUnit;
                soldier.Move(tile);
            }
        }
     
    }

    public Tile CreateTile(Vector3 position)
    {
        var tile = Instantiate(tilePrefab, boardParent);
        
        tile.transform.position = position;
        return tile;
    }
    
    private BoardUnit GetBoardUnitMouseOver()
    {
        var raycastHit = Physics2D.Raycast(MousePositionToWorldPosition(), Vector3.forward, 1000);
        if (raycastHit.collider == null)
            return null;

        return raycastHit.collider.gameObject.GetComponent<BoardUnit>();
    }
    private Tile GetTileMouseOver()
    {
        var raycastHit = Physics2D.Raycast(MousePositionToWorldPosition(), Vector3.forward, 1000);
        if (raycastHit.collider == null)
            return null;

        return raycastHit.collider.gameObject.GetComponent<Tile>();
    }
    private Vector3 MousePositionToWorldPosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool CheckPlaceable(BoardUnit boardUnit, Tile originTile)
    {
        var selectedPlaceableUnit = boardUnit;
        List<Tile> tileList = new();
        bool status = true;
        for (int i = 0; i < selectedPlaceableUnit.dimension.x; i++) 
        {
            for (int j = 0; j < selectedPlaceableUnit.dimension.y; j++)
            {
                var tile = originTile.GetNeighbourByDirection(i, j);
                if(tile != null)
                {
                    tileList.Add(tile);
                }
                else
                {
                    //Out of Board Bound
                    status = false;
                }
            }
        }
        var filledTileExist = tileList.Find(a => !a.isEmpty);
        if (filledTileExist)
        {
            //Filled file found
            foreach (var item in tileList)
            {
                if(filledTileExist && !item.isEmpty)
                { 
                    item.ErrorHighlight(Color.red);
                }
                else if(item.isEmpty)
                {
                    item.ErrorHighlight(Color.green);
                }
            }
            return false;
        }

        //Out of Board Bound
        if (!status)
        {
            foreach (var item in tileList)
            {
                item.ErrorHighlight(Color.red);
            }
            return false;
        }
        return true;
    }

    /// <summary>
    /// Place
    /// </summary>
    /// <param name="originTile"></param>
    private void PlaceSelectedUnitToTile(Tile originTile)
    {
        //var selectedPlaceableUnit = Instantiate(ProductionManager.Instance.selectedPlaceableUnit, boardParent);
        var selectedPlaceableUnit = BoardUnitObjectPool.Instance.GetNextBoardUnit(ProductionManager.Instance.selectedPlaceableUnit.unitName);
        selectedPlaceableUnit.gameObject.SetActive(true);
        Vector2 offset = (selectedPlaceableUnit.dimension / 2.0f);
        offset.x -= board.tileSize / 2.0f;
        offset.y -= board.tileSize / 2.0f;
        selectedPlaceableUnit.transform.position = (Vector2)originTile.transform.position + offset;

        for (int i = 0; i < selectedPlaceableUnit.dimension.x; i++)
        {
            for (int j = 0; j < selectedPlaceableUnit.dimension.y; j++)
            {
                var tile = originTile.GetNeighbourByDirection(i, j);
                tile.isEmpty = false;
            }
        }

        selectedPlaceableUnit.Placed(originTile);
        ProductionManager.Instance.ResetSelectedPlaceableUnit();
    }

    public void RemoveUnit(BoardUnit boardUnit)
    {
        for (int i = 0; i < boardUnit.dimension.x; i++)
        {
            for (int j = 0; j < boardUnit.dimension.y; j++)
            {
                var tile = boardUnit.originTile.GetNeighbourByDirection(i, j);
                tile.isEmpty = true;
            }
        }
    }
}
