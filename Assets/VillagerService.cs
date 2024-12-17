using System.Collections;
using System.Collections.Generic;
using Script.Architecture;
using UnityEngine;

public class VillagerService : Singleton<VillagerService> //Вынести интерфейс IValut
{
    [SerializeField] private int _freeVillager;

    public int FreeVillager
    {
        get => _freeVillager;
        set
        {
            if (value >= 0)
            {
                UIEvents.OnFreeVillagerChanged?.Invoke(value);
                _freeVillager = value;
            }
        }
    }
}
