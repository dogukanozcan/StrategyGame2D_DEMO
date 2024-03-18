using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanelProduct : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private TextMeshProUGUI damageLabel;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    [HideInInspector] public Soldier soldier;
    
    public void Setup(Soldier soldier)
    {
        this.soldier = soldier;
        image.sprite = soldier.mainSpriteRenderer.sprite;
        nameLabel.text = soldier.unitName;
        healthLabel.text = "Health: " + soldier.maxHealth.ToString();
        damageLabel.text = "Damage: " + soldier.damage.ToString();
        button.onClick.AddListener(() => { Spawn(); });
    }
    public void Spawn()
    {
        SoldierSpawner.Instance.SpawnSoldierSelectedBoardUnit((Soldier)BoardUnitObjectPool.Instance.GetNextBoardUnit(soldier.unitName));
    }
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
