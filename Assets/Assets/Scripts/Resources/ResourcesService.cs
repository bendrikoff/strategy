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
            };
        }
    }

    public enum ResourceType
    {
        Money, Villagers, Food
    }
}