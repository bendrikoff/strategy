using System.Collections.Generic;
using System.Linq;
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
        var state = JsonUtility.FromJson<StateWrapper>(json); // Десериализация
        RestoreGameState(state);
    }

    private StateWrapper CaptureGameState()
    {
        var state = new StateWrapper();
        foreach (var saveable in Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            state.Objects.Add(new SaveableState
            {
                ObjectType = saveable.GetType().FullName,
                State = JsonUtility.ToJson(saveable.CaptureState())
            });
        }
        return state;
    }

    private void RestoreGameState(StateWrapper state)
    {
        foreach (var saveable in Object.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            var saveableType = saveable.GetType().FullName;
            var savedState = state.Objects.Find(obj => obj.ObjectType == saveableType);

            if (savedState != null)
            {
                var stateData = JsonUtility.FromJson(savedState.State, saveable.CaptureState().GetType());
                saveable.RestoreState(stateData);
            }
        }
    }

    [System.Serializable]
    private class StateWrapper
    {
        public List<SaveableState> Objects = new List<SaveableState>();
    }

    [System.Serializable]
    private class SaveableState
    {
        public string ObjectType;
        public string State;
    }
}
