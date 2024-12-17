using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entity.Buildings;
using UnityEngine;

public class UIEvents
{
    public static Action<Building> OnSelectedBuilding;
    
    public static Action OnStartBuilding;
    public static Action OnEndBuilding;

    public static Action<int> OnMoneyChanged;
    public static Action OnShowNoMoney;
    
    public static Action<int> OnFreeVillagerChanged;
}
