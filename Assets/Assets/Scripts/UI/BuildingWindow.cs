using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingWindow : MonoBehaviour
{
    public GameObject Panel;
    public TextMeshProUGUI TextName;
    public Image Icon;
    public List<BuildingUI> BuildingUIList;

    private Building _selectedBuilding;

    private void OnEnable()
    {
        UIEvents.OnSelectedBuilding += Open;
    }

    private void OnDisable()
    {
        UIEvents.OnSelectedBuilding -= Open;
    }
    private void Open(Building building)
    {
        _selectedBuilding = building;
        Panel.SetActive(true);
        var uiData = BuildingUIList.First(x => x.Type == building.GetData().Type);
        TextName.text = uiData.TextName;
        Icon.sprite = uiData.Icon;
    }

    [Serializable]
    public class BuildingUI
    {
        public BuildingType Type;
        public Sprite Icon;
        public string TextName;
        public int BuyPrice;
    }
}
