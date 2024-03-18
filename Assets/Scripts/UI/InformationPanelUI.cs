using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPanelUI : MonoBehaviour
{
    public static InformationPanelUI Instance;

    private const string panelSlideInAnimationName = "InformationPanelSlideIn";
    private const string panelSlideOutAnimationName = "InformationPanelSlideOut";
    private bool panelIsActive = false;
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI otherInfoLabel;
    [SerializeField] private TextMeshProUGUI dimensionLabel;
    [SerializeField] private Image boardUnitImage;
    [SerializeField] private RectTransform productArea;
    [SerializeField] private Outline spawnPointButtonOutline;


    [SerializeField] private InformationPanelProduct informationPanelProductPrefab;

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
    private void Update()
    {
       
    }
    public void OpenPanel(BoardUnit boardUnit)
    {
        productArea.gameObject.SetActive(false);

        if (!panelIsActive)
        {
            panelAnimator.Play(panelSlideInAnimationName);
            panelIsActive = true;
        }

        nameLabel.text = boardUnit.unitName;
        otherInfoLabel.text = boardUnit.ToString();
        boardUnit.OnHealthChange += () => otherInfoLabel.text = boardUnit.ToString();

        dimensionLabel.text = boardUnit.dimension.x + "x" + boardUnit.dimension.y;
        boardUnitImage.sprite = boardUnit.mainSpriteRenderer.sprite;


        if (boardUnit.GetType() == typeof(Barracks))
        {
            var barracks = (Barracks)boardUnit;
            foreach (var item in barracks.productList)
            {
                var product = Instantiate(informationPanelProductPrefab, productArea);
                product.Setup((Soldier)item);
            }
            productArea.gameObject.SetActive(true);
        }
    }
    public void ClosePanel()
    {
        if (panelIsActive)
        {
            panelAnimator.Play(panelSlideOutAnimationName);
            panelIsActive = false;
        }
        ChangeSpawnPointStatus(false);
    }

    public void ChangeSpawnPointButton()
    {
        ChangeSpawnPointStatus(!BoardManager.Instance.spawnPointChange);
    }

    public void ChangeSpawnPointStatus(bool status)
    {
        spawnPointButtonOutline.enabled = status;
        BoardManager.Instance.spawnPointChange = status;
    }

}
