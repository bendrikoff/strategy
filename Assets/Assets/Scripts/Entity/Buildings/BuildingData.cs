using System;
using UnityEngine;

namespace Assets.Scripts.Entity.Buildings
{
    [Serializable]
    public struct BuildingData
    {
        public BuildingType Type;
        public Vector2 Position;
        public int BuildingStage;

        public BuildingData(int buildingStage, BuildingType type, Vector2 position)
        {
            BuildingStage = buildingStage;
            Type = type;
            Position = position;
        }
    }
}
