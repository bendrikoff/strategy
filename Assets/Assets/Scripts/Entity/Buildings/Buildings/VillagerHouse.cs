using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Buildings;
using Assets.Scripts.Resources;
using UnityEngine;

public class VillagerHouse : DraggableBuilding
{
    public int IncreaseVillagers;
    public override void Initialize()
    {
        ResourcesService.Instance.IncreaseResource(ResourceType.Villagers, IncreaseVillagers);
    }
}
