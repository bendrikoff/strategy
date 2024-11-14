namespace Assets.Scripts.StateMachine
{
    public interface IStateMachine
    { 
        void SetState(IState newState);
    }
}