using System.Collections.Generic;
using Script.Architecture;

namespace Assets.Scripts.Resources
{
    public class ResourcesService : Singleton<ResourcesService>
    {
        public Dictionary<ResourceType, IResource> Resources;

        protected override void Awake()
        {
            base.Awake();
            Resources = new Dictionary<ResourceType, IResource>
            {
                {ResourceType.Money, new Money()},
                {ResourceType.Villagers, new Villagers()},
                {ResourceType.Food, new Food()},
                {ResourceType.Log, new BaseResource("Дерево")},
            };
            Resources[ResourceType.Log].AddAmount(100);
        }

        public void IncreaseResource(ResourceType resource, int amount)
        {
            if(amount < 0)return;
            Resources[resource].AddAmount(amount);
        }

        public bool DecreaseResource(ResourceType resource, int amount)
        {
            return Resources[resource].RemoveAmount(amount);
        }

        public bool CheckRequirements(List<ResourceRequirements> requirements)
        {
            foreach (var requirement in requirements)
            {
                if (!Resources.TryGetValue(requirement.ResourceType, out var resource))
                {
                    return false;
                }
                if (resource.Amount < requirement.Count)
                {
                    return false;
                }
            }

            return true;
        }
    }
    
    
    public enum ResourceType
    {
        Money, Villagers, Food, Log
    }
}