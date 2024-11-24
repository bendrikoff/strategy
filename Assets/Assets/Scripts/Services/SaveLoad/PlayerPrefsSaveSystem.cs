using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Entity.Buildings;
using UnityEngine;

public class PlayerPrefsSaveSystem : ISaveSystem
{
    public void Save(string key)
    {
        var state = CaptureGameState();
        var json = JsonUtility.ToJson(state); // Сериализация
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void Load(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            Debug.LogWarning($"Save key \"{key}\" not found!");
            return;
        }

        var json = PlayerPrefs.GetString(key);
        var state = JsonUtility.FromJson<PlayerData>(json); 
        RestoreGameState(state);
    }

    private PlayerData CaptureGameState()
    {
        var data = new PlayerData();
        data.WorldData = new WorldData();
        data.WorldData.BuildingData = new List<BuildingData>();
        
        foreach (var saveable in Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            saveable.SaveState(data);
        }
        Debug.Log($"saved {data.WorldData.BuildingData.Count} buildings");
        return data;
    }

    private void RestoreGameState(PlayerData data)
    {
        Debug.Log($"loaded {data.WorldData.BuildingData.Count} buildings");
        foreach (var loadable in Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>()) 
        {
            loadable.LoadState(data);
        }
    }
}
