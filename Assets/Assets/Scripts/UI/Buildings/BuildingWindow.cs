using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Buildings;
using Assets.Scripts.Resources;
using Assets.Scripts.UI.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//todo:Рефактор
public class BuildingWindow : MonoBehaviour
{
    public GameObject Panel;

    public Transform RequirementsList;
    public TextMeshProUGUI TextName;
    public Image Icon;
    public BuildingButton BuildingButton;
    public List<BuildingUI> BuildingUIList;
    public BuildingUI SelectedBuilding;
    public BuildingFactory BuildingFactory;
    public BuildingTabs BuildingTabs;
    public GameObject BuildingList;
    
    private void OnEnable()
    {
        BuildingTabs.OnChangeBuildingTab += SetBuildingList;
    }

    private void OnDisable()
    {
        BuildingTabs.OnChangeBuildingTab -= SetBuildingList;
    }
    
    private void SelectBuilding(BuildingUI buildingUI)
    {
        ClearItemList();
        //todo: изменить логику(пс при открытии окна сразу подставляется SelectedBuilding почему-то)
        if (TextName.gameObject.activeSelf == false)
        {
            TextName.gameObject.SetActive(true);
        }

        if (Icon.gameObject.activeSelf == false)
        {
            Icon.gameObject.SetActive(true);
        }
        SelectedBuilding = buildingUI;
        TextName.text = buildingUI.TextName;
        Icon.sprite = buildingUI.Icon;

        foreach (var resource in buildingUI.Requirements)
        {
            var inventoryItem = ResourcesUIService.Instance.ResourceItemPrefab;
            var newItem = Instantiate(inventoryItem, RequirementsList);
            var resourceUI = ResourcesUIService.Instance.Resources.First(x => x.Type == resource.ResourceType);
            
            var resourceItem = newItem.GetComponent<ResourceItem>();
            resourceItem.Init(resourceUI.ItemIcon, resource.Count);
            if (resource.Count > ResourcesService.Instance.Resources[resource.ResourceType].Amount)
            {
                resourceItem.SetRedCountText();
            }
        }
    }

    private void ClearItemList()
    {
        foreach (Transform item in RequirementsList)
        {
            Destroy(item.gameObject);
        }
    }

    private bool CanBuild(BuildingUI buildingUI)
    {
        return ResourcesService.Instance.CheckRequirements(buildingUI.Requirements);
    }
    
    public void OnClickBuild()
    {
        if (SelectedBuilding != null && CanBuild(SelectedBuilding))
        {
            BuildingFactory.CreateBuilding(Vector3.zero, SelectedBuilding.Type);
            Panel.SetActive(false);
            UIEvents.OnStartBuilding?.Invoke();
        }
    }

    private void SetBuildingList(ProductionType tab)
    {
        foreach (Transform building in BuildingList.transform)
        {
            Destroy(building.gameObject);
        }
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
        public ProductionType Tab;
        public List<ResourceRequirements> Requirements;
    }
}