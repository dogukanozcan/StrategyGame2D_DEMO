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

    private void Start()
    {
        StartCoroutine(Attacker());
    }
    public void Move(Tile targetTile)
    {
        if (onMoving)
            return;

        onMoving = true;
        animator.SetBool(runBoolName, onMoving);

        var start = originTile;
        var end = targetTile;
        var path = Pathfinding.Instance.FindPath(start, end);

       StartCoroutine(Move(path));
    }

    public IEnumerator Move(List<Tile> path, float animationTimePerTile = .2f)
    {
        if(path != null)
        {
            foreach (var item in path)
            {
                transform.DOMove(item.transform.position, animationTimePerTile);
                Placed(item);
                yield return new WaitForSeconds(animationTimePerTile);
            }
        }
        animator.SetBool(runBoolName, false);
        onMoving = false;
    }

    public void SetTarget(BoardUnit target)
    {
        targetUnit = target;
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
            var raycastHit = Physics2D.Raycast(originTile.transform.position + (direction * (cellSize/2f))+ (direction/100f), direction, cellSize, unitMask);
            var unit = raycastHit.collider?.GetComponent<BoardUnit>();
            if(unit)
                list.Add(unit);
        }
        foreach (var item in diagonal)
        {
            var direction = item.normalized;
            var raycastHit = Physics2D.Raycast(originTile.transform.position + (direction * tileDiagonalLength/2f) + (direction / 100f), direction, tileDiagonalLength, unitMask);
            var unit = raycastHit.collider?.GetComponent<BoardUnit>();
            if (unit)
                list.Add(unit);
        }
        return list;
    }

    public List<BoardUnit> check;
    public IEnumerator Attacker()
    {
        while(true)
        {
            yield return new WaitUntil(() => targetUnit != null);
            // if(targetUnit.originTile.TileDistance(originTile) > 1.5f)

            check = NeighboursCheck();
            if (!check.Contains(targetUnit))
            {
                yield return new WaitForSeconds(.2f);
                continue;
            }
          
            targetUnit.TakeDamage(damage);
            animator.SetTrigger(attackTriggerName);
            if (targetUnit.isDead)
            {
                print("isDead NULL");
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
