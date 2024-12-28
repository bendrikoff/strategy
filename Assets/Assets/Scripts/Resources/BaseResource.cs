using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Resources;
using UnityEngine;

public class BaseResource : IResource
{
    public string Name { get; }
    public int Amount { get; set; }
    public ResourceType Type { get; set; }

    public BaseResource(string name)
    {
        Name = name;
    }
    
    public void AddAmount(int amount)
    {
        if(amount < 0) return; 
        Amount += amount;
    }

    public bool RemoveAmount(int amount)
    {
        throw new System.NotImplementedException();
    }
}
