using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Buildings;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingFactory : MonoBehaviour, ISaveable
{
    [Header("Префабы зданий")] //Возможно заменить на Scriptable object 
    public GameObject VillagerHouse;
    public GameObject Tavern;
    
    public IReadOnlyList<Building> Buildings => allBuildings.AsReadOnly();
    
    private List<Building> allBuildings = new List<Building>();
    
    private Dictionary<BuildingType, GameObject> _buildingPrefabs;

    private void Awake()
    {
        allBuildings = FindObjectsOfType<Building>().ToList();
        
        _buildingPrefabs = new Dictionary<BuildingType, GameObject>()
        {
            { BuildingType.VillagerHouse ,VillagerHouse}, 
            { BuildingType.Tavern, Tavern }
        };
        
        Create1();
        Create2();
    }

    public void Create1()
    {
        CreateBuilding(Vector3.zero,BuildingType.Tavern);
    }
    public void Create2()
    {
        CreateBuilding(Vector3.one, BuildingType.VillagerHouse);
    }

    
    public GameObject CreateBuilding(Vector3 position, BuildingType buildingType)
    {
        if (!_buildingPrefabs.ContainsKey(buildingType))
        {
            Debug.LogError("Invalid building type!");
            return null;
        }

        var prefab = _buildingPrefabs[buildingType];
        var newBuilding = Instantiate(prefab, position, Quaternion.identity);
        
        var buildingComponent = newBuilding.GetComponent<Building>();
        buildingComponent?.Initialize();

        RegisterBuilding(buildingComponent);
        return newBuilding;
    }

    public void RegisterBuilding(Building building)
    {
        if (!allBuildings.Contains(building))
            allBuildings.Add(building);
    }

    public void RemoveBuilding(Building building)
    {
        if (allBuildings.Contains(building))
            allBuildings.Remove(building);
    }

    public void SaveState(PlayerData data)
    {
        data.WorldData.BuildingData = allBuildings.Select(x=>x.GetData()).ToList();
    }

    public void LoadState(PlayerData data)
    {
        if (data?.WorldData?.BuildingData != null)
        {
            ClearBuildingList();

            foreach (var building in  data.WorldData.BuildingData)
            {
                CreateBuilding(building.Position, building.Type);
            }  
        }
    }

    private void ClearBuildingList()
    {
        for (int i = 0; i < allBuildings.Count; i++)
        {
            Destroy(allBuildings[i].GameObject());
        }

        allBuildings = new();
    }
}