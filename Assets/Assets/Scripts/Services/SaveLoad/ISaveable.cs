using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void SaveState(PlayerData data);
    
    void LoadState(PlayerData data);
}
