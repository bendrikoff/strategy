using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Resources;
using UnityEngine;

public class Food : IResource
{
    public string Name { get; }
    public int Amount { get; set; }
    public ResourceType Type { get; set; }

    public void AddAmount(int amount)
    {
        throw new System.NotImplementedException();
    }

    public bool RemoveAmount(int amount)
    {
        throw new System.NotImplementedException();
    }
}
