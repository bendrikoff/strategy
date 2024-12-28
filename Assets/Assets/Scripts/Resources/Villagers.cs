namespace Assets.Scripts.Resources
{
    public class Villagers : IResource
    {
        public string Name { get; }
        public int Amount { get; set; }
        public ResourceType Type { get; set; }

        public void AddAmount(int amount)
        {
             if(amount < 0) return;
             Amount += amount;
             UIEvents.OnFreeVillagerChanged?.Invoke(Amount);
        }

        public bool RemoveAmount(int amount)
        {
            if (Amount - amount < 0) return false;
            Amount -= amount;
            UIEvents.OnFreeVillagerChanged?.Invoke(Amount);
            return true;
        }
    }
}
