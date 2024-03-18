using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUI : MonoBehaviour
{
    public static BoardUI Instance;

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

    public void BoardUnitSelected(BoardUnit boardUnit)
    {
        ResetSelectedBoardUnit(false);
        BoardManager.Instance.selectedBoardUnit = boardUnit;
        BoardManager.Instance.selectedBoardUnit.outline.enabled = true;

        InformationPanelUI.Instance.OpenPanel(boardUnit);
    }

    public void ResetSelectedBoardUnit(bool closePanel = true)
    {
        if (BoardManager.Instance.selectedBoardUnit != null)
        {
            if (BoardManager.Instance.selectedBoardUnit.GetType() == typeof(Barracks))
            {
                Barracks barracks = (Barracks)BoardManager.Instance.selectedBoardUnit;
                barracks.ResetSelect();
            }
            BoardManager.Instance.selectedBoardUnit.OnHealthChange = null;
            BoardManager.Instance.selectedBoardUnit.outline.enabled = false;
            BoardManager.Instance.selectedBoardUnit = null;
        }

        InformationPanelUI.Instance.ChangeSpawnPointStatus(false);

        if (closePanel)
        {
            InformationPanelUI.Instance.ClosePanel();
        }
    }

}
