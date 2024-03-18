using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionMenuButton : MonoBehaviour
{
    [SerializeField] private BoardUnit boardUnit;

    [SerializeField] private Outline outline;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI dimensionLabel;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private Image boardUnitImage;
    
    

    private void Awake()
    {
        button.onClick.AddListener(() => ButtonClicked());
    }

    public void ButtonClicked()
    {
        BoardUI.Instance.ResetSelectedBoardUnit();
        ProductionMenuUI.Instance.SetSelectedPlaceableUnit(boardUnit, this);
    }

    public void SetSelectedStatus(bool isSelected)
    {
        if (outline != null)
        {
            outline.enabled = isSelected;
        }
        else
        {
            Debug.LogError("Outline not assigned");
        }
    }

    private void OnEnable()
    {
        boardUnitImage.sprite = boardUnit.mainSpriteRenderer.sprite;
        nameLabel.text = boardUnit.unitName.ToString();
        dimensionLabel.text = boardUnit.dimension.x + "x" + boardUnit.dimension.y;
    }
}
