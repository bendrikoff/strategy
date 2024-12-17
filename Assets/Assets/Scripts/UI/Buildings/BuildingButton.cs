using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Image BuildingImage;

    public TextMeshProUGUI Text;

    public Button Button;

    public void Init(BuildingWindow.BuildingUI buildingUI, UnityAction callback)
    {
        Text.text = buildingUI.TextName;
        BuildingImage.sprite = buildingUI.Icon;
        Button.onClick.AddListener(() =>
        {
            if (MoneyService.Instance.TryToBuy(buildingUI.BuyPrice))
            {
                callback();
            }
        });
    }
}
