using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Resources;
using Assets.Scripts.UI.Resources;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform ItemList;

    private List<ResourceType> _notPresentedInInventoryResources = new List<ResourceType>()
    {
        ResourceType.Food,
        ResourceType.Money,
        ResourceType.Villagers
    };

    private void Awake()
    {
        var inventoryResources =
            ResourcesService.Instance.Resources.Where(x => _notPresentedInInventoryResources.All(resource => resource != x.Key) && x.Value.Amount > 0);

        foreach (var resource in inventoryResources)
        {
            var inventoryItem = ResourcesUIService.Instance.ResourceItemPrefab;
            var newItem = Instantiate(inventoryItem, ItemList);
            var resourceUI = ResourcesUIService.Instance.Resources.First(x => x.Type == resource.Key);
            newItem.GetComponent<ResourceItem>().Init(resourceUI.ItemIcon, resource.Value.Amount);
        }
    }
}