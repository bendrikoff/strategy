using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public Button Button;

    public void Init(BuildingWindow.BuildingUI buildingUI, UnityAction callback)
    {
        Text.text = buildingUI.TextName;
        Button.onClick.AddListener(() =>
        {
            if (ResourcesService.Instance.DecreaseResource(ResourceType.Money, buildingUI.BuyPrice))
            {
                callback();
            }
        });
    }
}
