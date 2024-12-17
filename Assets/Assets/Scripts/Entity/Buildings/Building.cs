using System;
using System.Collections.Generic;
using Assets.Scripts.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entity.Buildings
{
    public abstract class Building : MonoBehaviour
    {
        public BuildingType Type;
        public int Stage;
        public int Width;
        public int Heigth;
        public List<ResourceRequirements> ResourceRequirements;

        public abstract void Initialize();

        public virtual BuildingData GetData()
        {
            return new BuildingData(Stage, Type, transform.position);
        }
    }
    [Serializable]
    public class ResourceRequirements
    {
        public ResourceType ResourceType;
        public int Count;
    }
}
