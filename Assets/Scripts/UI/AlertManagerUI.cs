using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertManagerUI : MonoBehaviour
{
    public static AlertManagerUI Instance { get; private set; }

    public TextMeshProUGUI messageLabel;
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

    public void HideAlert()
    {
        messageLabel.transform.parent.gameObject.SetActive(false);
    }

    public void ShowAlert(string message)
    {
        messageLabel.transform.parent.gameObject.SetActive(true);
        messageLabel.text = message;
    }
}
