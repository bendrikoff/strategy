using System.Collections;
using System.Collections.Generic;
using Script.Architecture;
using UnityEngine;

public class MoneyService : Singleton<MoneyService>
{
    public int Money
    {
        get => _money;
        set
        {
            if (value >= 0)
            {
                UIEvents.OnMoneyChanged?.Invoke(value);
                _money = value;
            }
        }
    }

    [SerializeField] private int _money;

    public bool TryToBuy(int price)
    {
        if (_money - price >= 0)
        {
            return true;
        }

        UIEvents.OnShowNoMoney?.Invoke();
        return false;
    }
}
