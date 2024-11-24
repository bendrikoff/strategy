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
        public abstract void Initialize();

        public virtual BuildingData GetData()
        {
            return new BuildingData(Stage, Type, transform.position);
        }
        
        private void OnMouseDown()
        {
            UIEvents.OnSelectedBuilding?.Invoke(this);
        }
    }
}
