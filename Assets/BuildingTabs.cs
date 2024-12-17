using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Buildings;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTabs : MonoBehaviour
{
    public Action<BuildingTabsType> OnChangeBuildingTab;
    public Sprite ActiveTab;
    public Sprite UnActiveTab;
    public List<UITab> Tabs;

    private Dictionary<BuildingTabsType, GameObject> _tabs;
    private BuildingTabsType _selectedTab;

    private void Awake()
    {
        _tabs = Tabs.ToDictionary(x => x.TabType, x => x.Tab);
        OpenTab(Tabs.First().TabType);

        foreach (var tab in _tabs)
        {
            tab.Value.GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenTab(tab.Key);
            });
        }
    }
    

    public void OpenTab(BuildingTabsType tabType)
    {
        //if(_selectedTab == tabType) return;
        OnChangeBuildingTab?.Invoke(tabType);
        Debug.Log(tabType);
        _tabs[tabType].GetComponent<Image>().sprite = ActiveTab;
        _tabs[_selectedTab].GetComponent<Image>().sprite = UnActiveTab;
        _selectedTab = tabType;
    }

    [Serializable]
    public class UITab
    {
        public GameObject Tab;
        public BuildingTabsType TabType;
    }
}
[Serializable]
public enum BuildingTabsType
{
    Houses, Craft, Production
}
