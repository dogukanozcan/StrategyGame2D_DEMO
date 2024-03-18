using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HighlightType
{
    unsuitable,
    suitable,
    mouseover,
    normal
}

public class Tile : MasterMonoBehaviour
{
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private SpriteRenderer alertImage;
    [HideInInspector] public Vector2Int index;

    public bool isEmpty = true;

    private TweenerCore<Color,Color,ColorOptions> errorTweener;

    [HideInInspector] public int F;
    [HideInInspector] public int H;
    [HideInInspector] public int G;
    [HideInInspector] public Tile parent;
    
    public void SetupTile(Vector2Int index)
    {
        this.index = index;
        isEmpty = true;
    }
   
    public void ErrorHighlight(Color color)
    {
        if(alertImage != null)
        {
            color.a = .5f;
            alertImage.enabled = true;
            alertImage.color = color;
          
            if (errorTweener != null && errorTweener.IsPlaying())
                errorTweener.Kill();

            errorTweener = alertImage.DOFade(0,.5f).OnComplete(() => alertImage.enabled = false);
        }
    }

    public Tile GetNeighbourByDirection(int x, int y)
    {
        return BoardManager.Instance.board.GetTile(index.x + x, index.y + y);
    }

    public float TileDistance(Tile target)
    {
        return Vector2Int.Distance(index, target.index);
    }

    #region PathFinding
    private static int ComputeHScore(int x, int y, int targetX, int targetY)
    {
        return Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y);
    }

    #endregion
}
