using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedBuildingWindow : MonoBehaviour
{
    public BuildingWindow BuildingWindow;
    
    public Image CurrentLevelImage;
    public TextMeshProUGUI CurrentLevelDescription;
    public TextMeshProUGUI BuildingName;
    
    public Image NextLevelImage;
    public TextMeshProUGUI NextLevel;

    private Building SelectedBuilding;
    private BuildingWindow.BuildingUI SelectedBuildingUI;

    public void Open(Building building)
    {
        gameObject.SetActive(true);
        SelectedBuilding = building;
        SelectedBuildingUI = BuildingWindow.BuildingUIList.First(x => x.Type == SelectedBuilding.Type);

        CurrentLevelImage.sprite = SelectedBuildingUI.Icon;
        BuildingName.text = SelectedBuildingUI.TextName;
        CurrentLevelDescription.text = $"Уровень: {building.Stage}";
    }
}
