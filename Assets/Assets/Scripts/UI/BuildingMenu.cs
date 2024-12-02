using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{
    public Button Button;
    public BuildingWindow BuildingWindow;
    public BuildingFactory BuildingFactory;
    private void Awake()
    {
        foreach (var building in BuildingWindow.BuildingUIList)
        {
            var button = GetBuildingButton(Button,building);
            Instantiate(button, transform);
        }
    }

    private Button GetBuildingButton(Button button, BuildingWindow.BuildingUI buildingUI)
    {
        var text = button.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = buildingUI.TextName;
        }

        if (button.TryGetComponent<Image>(out var image))
        {
            image.sprite = buildingUI.Icon;
        }
        button.onClick.AddListener(() =>
        {
            Debug.Log("Кнопка нажата!");
            BuildingFactory.Create1();
        });
        
        return button;
    }
}
