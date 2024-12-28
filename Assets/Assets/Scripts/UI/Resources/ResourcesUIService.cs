using System.Collections.Generic;
using Script.Architecture;
using UnityEngine;

namespace Assets.Scripts.UI.Resources
{
    public class ResourcesUIService : Singleton<ResourcesUIService>
    {
        public ResourceItem ResourceItemPrefab;
        public List<ResourceUI> Resources;
    }
}