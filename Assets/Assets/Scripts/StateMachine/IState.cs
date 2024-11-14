namespace Assets.Scripts.StateMachine
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }
}