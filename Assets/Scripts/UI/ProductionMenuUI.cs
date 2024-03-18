using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionMenuUI : MonoBehaviour
{
    public static ProductionMenuUI Instance;
    
    private ProductionMenuButton selectedButton;
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

    public void SetSelectedPlaceableUnit(BoardUnit boardUnit, ProductionMenuButton selectedButton)
    {
        if (this.selectedButton != null)
        {
            this.selectedButton.SetSelectedStatus(false);
        }
        this.selectedButton = selectedButton;
        this.selectedButton.SetSelectedStatus(true);

        ProductionManager.Instance.SetSelectedPlaceableUnit(boardUnit);
    }
    public void ResetSelectedPlaceableUnit()
    {
        if (selectedButton != null)
        {
            selectedButton.SetSelectedStatus(false);
        }
        selectedButton = null;
    }


}
