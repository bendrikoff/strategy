namespace Assets.Scripts.Resources
{
    public class Villagers : IResource
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
}
