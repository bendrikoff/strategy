namespace Assets.Scripts.Resources
{
    public interface IResource
    {
        string Name { get; }
        int Amount { get; set; }
        ResourceType Type { get; set; }
        void AddAmount(int amount);
        bool RemoveAmount(int amount);
    }
}