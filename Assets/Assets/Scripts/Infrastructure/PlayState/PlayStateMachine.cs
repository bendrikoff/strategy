using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.StateMachine;
using UnityEngine;

public class PlayStateMachine : IStateMachine
{
    public IState _currentState;

    public void Update()
    {
        _currentState?.Execute();
    }
    public void SetState(IState newState)
    {
        _currentState?.Exit();
        newState.Enter();
        _currentState = newState;
    }
}
