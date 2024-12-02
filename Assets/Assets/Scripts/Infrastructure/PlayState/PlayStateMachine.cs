using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.StateMachine;
using UnityEngine;

public class PlayStateMachine : IStateMachine
{
    public IState _currentState;

    public PlayStateMachine()
    {
        var idleState = new PlayingPlayState(BuildingReferences.Instance.CameraController);
        var buildingState = new BuildingPlayState(BuildingReferences.Instance.GridData, BuildingReferences.Instance.CameraController);
        SetState(buildingState);
    }

    public void Update()
    {
        _currentState?.Execute();
    }
    public void SetState(IState newState)
    {
        _currentState?.Enter();
        newState.Enter();
        _currentState = newState;
    }
}
