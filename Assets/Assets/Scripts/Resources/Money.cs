using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Resources;
using UnityEngine;

public class Money : IResource
{
    public string Name { get; }
    public int Amount { get; set; }
    public ResourceType Type { get; set; }

    public void AddAmount(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Используй метод RemoveAmount");
            return;
        }

        Amount += amount;
        UIEvents.OnMoneyChanged?.Invoke(Amount);
    }

    public bool RemoveAmount(int amount)
    {
        if (Amount + amount > 0)
        {
            UIEvents.OnShowNoMoney?.Invoke();
            return false;
        }

        Amount -= amount;
        UIEvents.OnMoneyChanged?.Invoke(Amount);
        return true;
    }
}
