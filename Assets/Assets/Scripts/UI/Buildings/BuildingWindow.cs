using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Buildings;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingWindow : MonoBehaviour
{
    public GameObject Panel;
    
    public TextMeshProUGUI TextName;
    public Image Icon;
    public BuildingButton BuildingButton;
    public List<BuildingUI> BuildingUIList;
    public BuildingUI SelectedBuilding;
    public BuildingFactory BuildingFactory;
    public BuildingTabs BuildingTabs;
    public GameObject BuildingList;
    
    private Building _selectedBuilding;

    private void OnEnable()
    {
        UIEvents.OnSelectedBuilding += Open;
        BuildingTabs.OnChangeBuildingTab += SetBuildingList;
    }

    private void OnDisable()
    {
        UIEvents.OnSelectedBuilding -= Open;
        BuildingTabs.OnChangeBuildingTab -= SetBuildingList;
    }
    
    private void Open(Building building)
    {
        _selectedBuilding = building;
        Panel.SetActive(true);
    }
    
    private void SelectBuilding(BuildingUI buildingUI)
    {
        SelectedBuilding = buildingUI;
        TextName.text = buildingUI.TextName;
        Icon.sprite = buildingUI.Icon;
    }
    
    public void OnClickBuild()
    {
        if (SelectedBuilding != null)
        {
            BuildingFactory.CreateBuilding(Vector3.zero, SelectedBuilding.Type);
            gameObject.SetActive(false);
            UIEvents.OnStartBuilding?.Invoke();
        }
    }

    private void SetBuildingList(BuildingTabsType tab)
    {
        foreach (var tabBuildings in BuildingUIList.Where(x=>x.Tab == tab))
        {
            var button = Instantiate(BuildingButton, BuildingList.transform).GetComponent<BuildingButton>();
            button.Init(tabBuildings, () => SelectBuilding(tabBuildings));
        }
    }

    public void OnClickMoveBuilding() => UIEvents.OnStartBuilding?.Invoke();

    [Serializable]
    public class BuildingUI
    {
        public BuildingType Type;
        public Sprite Icon;
        public string TextName;
        public int BuyPrice;
        public BuildingTabsType Tab;
    }
}
