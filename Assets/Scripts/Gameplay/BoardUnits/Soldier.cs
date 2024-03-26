using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Soldier : BoardUnit
{
    public int damage = 5;

    private const string attackTriggerName = "Attack";
    private const string runBoolName = "Run";

    [SerializeField] private Animator animator;

    public BoardUnit targetUnit = null;

    public LayerMask unitMask;

    private bool onMoving = false;

    public List<BoardUnit> check;

    private Coroutine attackerCoroutine;

    private Coroutine moveCoroutine;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        attackerCoroutine = StartCoroutine(Attacker());
    }

    private void OnDisable()
    {
        StopCoroutine(attackerCoroutine);
        attackerCoroutine = null;
    }

    private void Update()
    {
      
    }

    
    public void Move(Tile targetTile)
    {
        if (onMoving)
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            animator.SetBool(runBoolName, false);
            onMoving = false;
        }
            

        if (targetTile == null)
            return;

        onMoving = true;

        targetTile.ErrorHighlight(Color.green);
        
        animator.SetBool(runBoolName, onMoving);

        var start = originTile;
        var end = targetTile;
        var path = Pathfinding.Instance.FindPath(start, end);

        moveCoroutine = StartCoroutine(Move(path,targetTile));
    }

    public IEnumerator Move(List<Tile> path, Tile targetTile, float animationTimePerTile = .2f)
    {
        bool failedToMove = false;
        if(path != null)
        {
            foreach (var item in path)
            {
                if (!item.isEmpty)
                {
                    failedToMove = true;
                    break;
                }
                transform.DOMove(item.transform.position, animationTimePerTile);
                originTile.isEmpty = true;
                item.isEmpty = false;
                Placed(item);
                yield return new WaitForSeconds(animationTimePerTile);
            }
        }

        
        animator.SetBool(runBoolName, false);
        onMoving = false;

        if(failedToMove)
            Move(targetTile);
    }

    public void SetTarget(BoardUnit target)
    {
        targetUnit = target;

        if (target == null)
            return;

        if(originTile.index.x < target.originTile.index.x)
        {
            mainSpriteRenderer.flipX = false;
        }
        else
        {
            mainSpriteRenderer.flipX = true;
        }
        
    }

    public List<BoardUnit> NeighboursCheck()
    {
        List<BoardUnit> list = new();
        var ortogonal = new List<Vector3>() 
        {
            Vector3.left,
            Vector3.up,
            Vector3.down,
            Vector3.right,
        };
        var diagonal = new List<Vector3>()
        {
            Vector3.right + Vector3.up,
            Vector3.right + Vector3.down,
            Vector3.left + Vector3.up,
            Vector3.left + Vector3.down,
        };
        float cellSize = BoardManager.Instance.board.tileSize;
        float tileDiagonalLength = Mathf.Sqrt(Mathf.Pow(cellSize, 2) + Mathf.Pow(cellSize, 2));
        foreach (var item in ortogonal)
        {
            var direction = item.normalized;
           // Debug.DrawRay(originTile.transform.position + (direction * (cellSize / 2f)) + (direction / 100f), direction);
            var raycastHit = Physics2D.Raycast(originTile.transform.position + (direction * (cellSize/2f))+ (direction/100f), direction, cellSize, unitMask);
            var unit = raycastHit.collider?.GetComponent<BoardUnit>();
            if(unit)
                list.Add(unit);
        }
        foreach (var item in diagonal)
        {
            var direction = item.normalized;
            //Debug.DrawRay(originTile.transform.position + (direction * (cellSize / 2f)) + (direction / 100f), direction);
            var raycastHit = Physics2D.Raycast(originTile.transform.position + (direction * tileDiagonalLength/2f) + (direction / 100f), direction, tileDiagonalLength, unitMask);
            var unit = raycastHit.collider?.GetComponent<BoardUnit>();
            if (unit)
                list.Add(unit);
        }
        return list;
    }


    public IEnumerator Attacker()
    {
        while(true)
        {
            yield return new WaitUntil(() => targetUnit != null);
            // if(targetUnit.originTile.TileDistance(originTile) > 1.5f)

            check = NeighboursCheck();
            if (!check.Contains(targetUnit))
            {
                yield return new WaitForSeconds(.05f);
                continue;
            }
          
            targetUnit.TakeDamage(damage);
            animator.SetTrigger(attackTriggerName);
            if (targetUnit.isDead)
            {
                targetUnit = null;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public override string ToString()
    {
        return base.ToString() + "\r\n" + "Damage: " + damage;
    }

 
}
