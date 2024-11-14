using Assets.Scripts.StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using IState = Assets.Scripts.StateMachine.IState;

namespace Assets.Scripts.Camera
{
    public class CameraStateMachine : IStateMachine
    {
        private IState _currentState;

        public void SetState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState?.Execute();
        }
    }
}
